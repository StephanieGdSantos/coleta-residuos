# Coleta de Resíduos - API .NET 8

Este projeto é uma API RESTful desenvolvida em .NET 8 para gerenciamento de resíduos, pontos de coleta, agendamento de coletas e alertas ambientais. O sistema permite o cadastro, consulta, atualização e exclusão de resíduos e pontos de coleta, além de funcionalidades para agendar coletas e emitir alertas.

## Funcionalidades

- **Resíduos**: Cadastro, listagem, atualização e exclusão de resíduos.
- **Pontos de Coleta**: Gerenciamento de pontos de coleta, incluindo associação de resíduos.
- **Coletas Agendadas**: Agendamento e gerenciamento de coletas em pontos específicos.
- **Alertas**: Emissão de alertas relacionados a pontos de coleta.
- **Relacionamentos**: Consulta de resíduos por ponto de coleta e vice-versa.
- **Paginação**: Todas as listagens suportam paginação via parâmetros de query.

## Tecnologias Utilizadas

- [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Entity Framework Core](https://learn.microsoft.com/ef/core/)
- [Oracle Database](https://www.oracle.com/database/)
- [AutoMapper](https://automapper.org/)
- [Swagger/OpenAPI](https://swagger.io/tools/open-source/open-api/)

## Estrutura do Projeto

- **Controllers**: Endpoints da API.
- **Models**: Modelos de dados.
- **ViewModels**: Modelos para entrada e saída de dados.
- **Services**: Lógica de negócio.
- **Repository**: Acesso a dados.
- **Contexts**: Contexto do banco de dados.

## Configuração

1. **Banco de Dados**  
   Configure a string de conexão Oracle no arquivo `appsettings.Development.json`:
```
"ConnectionStrings": { "DatabaseConnection": "Data Source=...;User ID=...;Password=...;" }
```


2. **Dependências**  
   Restaure os pacotes NuGet:
```
dotnet restore
```


3. **Execução**  
   Inicie a aplicação:
```
dotnet run
```


   A API estará disponível em `https://localhost:5001` (ou porta configurada).

4. **Swagger**  
   Acesse a documentação interativa em `/swagger` durante o desenvolvimento.

## Exemplos de Endpoints

- `GET /api/Residuo` — Lista resíduos (com paginação)
- `POST /api/Residuo` — Cria um novo resíduo
- `GET /api/PontoColeta` — Lista pontos de coleta
- `POST /api/PontoColeta` — Cria um novo ponto de coleta
- `GET /api/PontoColeta/{id}/Residuo` — Lista resíduos de um ponto de coleta

## Observações

- O projeto utiliza AutoMapper para conversão entre Models e ViewModels.
- A autenticação está preparada, mas comentada nos controllers (`[Authorize]`).
- O código segue boas práticas de injeção de dependência e separação de responsabilidades.

## Contribuição

Pull requests são bem-vindos! Para grandes mudanças, abra uma issue primeiro para discutir o que você gostaria de modificar.

---

**Desenvolvido para fins acadêmicos.**