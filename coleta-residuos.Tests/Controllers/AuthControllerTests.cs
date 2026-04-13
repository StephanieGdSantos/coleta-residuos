using Xunit;
using Microsoft.AspNetCore.Mvc;
using coleta_residuos.Controllers;
using coleta_residuos.Models;
using coleta_residuos.Tests.Fixtures;

namespace coleta_residuos.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            var mockJwtSettings = MockFactory.CreateJwtSettingsMock();
            _controller = new AuthController(mockJwtSettings.Object);
        }

        [Fact]
        public void Login_DeveRetornar200_QuandoUsuarioForValido()
        {
            // Arrange
            var usuario = new UserModel { Username = "operador01", Password = "pass123" };

            // Act
            var resultado = _controller.Login(usuario);

            // Assert
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact]
        public void Login_DeveRetornar401_QuandoUsuarioForInvalido()
        {
            // Arrange
            var usuario = new UserModel { Username = "usuario_invalido", Password = "senha_errada" };

            // Act
            var resultado = _controller.Login(usuario);

            // Assert
            Assert.IsType<UnauthorizedResult>(resultado);
        }

        [Fact]
        public void Login_DeveRetornarTokenEmOk_QuandoLoginForBemSucedido()
        {
            // Arrange
            var usuario = new UserModel { Username = "operador01", Password = "pass123" };

            // Act
            var resultado = _controller.Login(usuario);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.NotNull(okResult.Value);
            
            // Verificar se a resposta contém um token
            var tokenResponse = okResult.Value.ToString();
            Assert.NotNull(tokenResponse);
        }
    }
}
