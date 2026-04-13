# Guia de Injeção de Dependência em Testes

## Visão Geral

Este projeto utiliza **injeção de dependência (DI)** na camada de controllers. Os testes devem seguir o mesmo padrão.

---

## 📋 Padrão de Uso General

### 1️⃣ Usar o `MockFactory`

Para evitar duplicação de código, use o factory auxiliar:

```csharp
using coleta_residuos.Tests.Fixtures;

var mockJwtSettings = MockFactory.CreateJwtSettingsMock();
var controller = new AuthController(mockJwtSettings.Object);
```

### 2️⃣ Criar Instância com Secret Customizado

Para testar comportamentos com secrets diferentes:

```csharp
var mockJwtSettings = MockFactory.CreateJwtSettingsMock("my-custom-secret-key");
var controller = new AuthController(mockJwtSettings.Object);
```

### 3️⃣ Criar Instância com Objeto JwtSettings Completo

Para casos mais complexos:

```csharp
var jwtSettings = new JwtSettings { Secret = "test-secret" };
var mockJwtSettings = MockFactory.CreateJwtSettingsMock(jwtSettings);
var controller = new AuthController(mockJwtSettings.Object);
```

---

## 🧪 Exemplo Completo

```csharp
using Xunit;
using Microsoft.AspNetCore.Mvc;
using coleta_residuos.Controllers;
using coleta_residuos.Models;
using coleta_residuos.Tests.Fixtures;

public class AuthControllerTests
{
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        // Setup - Usar factory para criar mock
        var mockJwtSettings = MockFactory.CreateJwtSettingsMock();
        _controller = new AuthController(mockJwtSettings.Object);
    }

    [Fact]
    public void Login_DeveRetornarToken_QuandoCredenciaisForemValidas()
    {
        // Arrange
        var usuario = new UserModel 
        { 
            Username = "operador01", 
            Password = "pass123" 
        };

        // Act
        var resultado = _controller.Login(usuario);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(resultado);
        var tokenResponse = okResult.Value as dynamic;
        Assert.NotNull(tokenResponse?.Token);
    }
}
```

---

## 🔧 Integrando com Outros Controllers

Quando um novo controller precisar de `JwtSettings`:

### 1. Criar o serializer (se necessário)
```csharp
public class MeuController : ControllerBase
{
    private readonly JwtSettings _jwtSettings;

    public MeuController(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }
    
    // ... resto do código
}
```

### 2. Testar sem modificar a classe
```csharp
var mockJwtSettings = MockFactory.CreateJwtSettingsMock();
var controller = new MeuController(mockJwtSettings.Object);
```

---

## 📦 Estrutura de Arquivos

```
coleta-residuos.Tests/
├── Controllers/
│   ├── AuthControllerTests.cs ✅ (Atualizado com DI)
│   ├── ResiduoControllerTests.cs
│   └── ...
├── Fixtures/
│   └── MockFactory.cs ✅ (Factory de mocks)
└── ...
```

---

## ✅ Boas Práticas

1. **Sempre use `MockFactory`** para criar mocks de `JwtSettings`
2. **Não hardcode secrets** nos testes (use o factory)
3. **Para testes com secrets diferentes**: Passe como parâmetro ao factory
4. **Documente** se um teste precisa de um tipo especial de configuração

---

## 🚀 Executar os Testes

```powershell
# Rodar todos os testes
dotnet test

# Apenas testes de Auth
dotnet test --filter "FullyQualifiedName~AuthControllerTests"

# Com cobertura
dotnet test /p:CollectCoverage=true
```

---

## 📝 Adicionar Novos Mocks

Para adicionar novos mocks ao factory (ex: `DatabaseSettings`):

```csharp
public static Mock<IOptions<DatabaseSettings>> CreateDatabaseSettingsMock(
    string connectionString = null)
{
    var mockDbSettings = new Mock<IOptions<DatabaseSettings>>();
    var connStr = connectionString ?? "default-connection-string";
    
    mockDbSettings.Setup(x => x.Value).Returns(new DatabaseSettings
    {
        ConnectionString = connStr
    });

    return mockDbSettings;
}
```
