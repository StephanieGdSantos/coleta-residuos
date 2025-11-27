using Xunit;
using Microsoft.AspNetCore.Mvc;
using coleta_residuos.Controllers;
using coleta_residuos.Models;

namespace coleta_residuos.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _controller = new AuthController();
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
    }
}
