using Xunit;
using Moq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using coleta_residuos.Controllers;
using coleta_residuos.Models;
using coleta_residuos.Services;
using coleta_residuos.ViewModel;
using System;

namespace coleta_residuos.Tests.Controllers
{
    public class AlertaControllerTests
    {
        private readonly Mock<IAlertaService> _alertaServiceMock;
        private readonly Mock<IService<PontoColetaModel>> _pontoColetaServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AlertaController _controller;

        public AlertaControllerTests()
        {
            _alertaServiceMock = new Mock<IAlertaService>();
            _pontoColetaServiceMock = new Mock<IService<PontoColetaModel>>();
            _mapperMock = new Mock<IMapper>();

            _controller = new AlertaController(_alertaServiceMock.Object,
                _pontoColetaServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public void Get_DeveRetornar200_ComListaDeAlertas()
        {
            // Arrange
            var alertas = new List<AlertaModel>();
            var alertasViewModel = new List<AlertaViewModel>();
            _alertaServiceMock.Setup(s => s.Listar(0, 10)).Returns(alertas);
            _mapperMock.Setup(m => m.Map<IEnumerable<AlertaViewModel>>(alertas)).Returns(alertasViewModel);

            // Act
            var resultado = _controller.Get();

            // Assert
            Assert.IsType<OkObjectResult>(resultado.Result);
        }

        [Fact]
        public void Get_DeveRetornar400_QuandoExcecaoForLancada()
        {
            // Arrange
            _alertaServiceMock.Setup(s => s.Listar(0, 10)).Throws(new Exception("Erro"));

            // Act
            var resultado = _controller.Get();

            // Assert
            Assert.IsType<BadRequestObjectResult>(resultado.Result);
        }

        [Fact]
        public void GetPorId_DeveRetornar200_ComAlertaExistente()
        {
            // Arrange
            var alerta = new AlertaModel { Id = 1 };
            var alertaViewModel = new AlertaViewModel { Id = 1 };
            _alertaServiceMock.Setup(s => s.ObterPorId(1)).Returns(alerta);
            _mapperMock.Setup(m => m.Map<AlertaViewModel>(alerta)).Returns(alertaViewModel);

            // Act
            var resultado = _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            Assert.Equal(alertaViewModel, okResult.Value);
        }

        [Fact]
        public void GetPorId_DeveRetornar404_QuandoAlertaNaoExistir()
        {
            // Arrange
            _alertaServiceMock.Setup(s => s.ObterPorId(1)).Returns((AlertaModel)null);

            // Act
            var resultado = _controller.Get(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(resultado.Result);
            Assert.Equal("Alerta não encontrado.", notFoundResult.Value);
        }

        [Fact]
        public void GetPorId_DeveRetornar400_QuandoExcecaoForLancada()
        {
            // Arrange
            _alertaServiceMock.Setup(s => s.ObterPorId(1)).Throws(new Exception("Erro"));

            // Act
            var resultado = _controller.Get(1);

            // Assert
            Assert.IsType<BadRequestObjectResult>(resultado.Result);
        }

        [Fact]
        public void Post_DeveRetornar201_ComAlertaCriado()
        {
            // Arrange
            var criarAlertaViewModel = new CriarAlertaViewModel { PontoColetaId = 1, Mensagem = "Teste" };
            var alertaModel = new AlertaModel { Id = 1, PontoColetaId = 1, Mensagem = "Teste" };
            var alertaViewModel = new AlertaViewModel { Id = 1, PontoColetaId = 1, Mensagem = "Teste" };
            _mapperMock.Setup(m => m.Map<AlertaModel>(criarAlertaViewModel)).Returns(alertaModel);
            _pontoColetaServiceMock.Setup(s => s.ObterPorId(1)).Returns(new PontoColetaModel { Id = 1 });
            _mapperMock.Setup(m => m.Map<AlertaViewModel>(alertaModel)).Returns(alertaViewModel);

            // Act
            var resultado = _controller.Post(criarAlertaViewModel);

            // Assert
            _alertaServiceMock.Verify(s => s.Criar(It.IsAny<AlertaModel>()), Times.Once);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(resultado.Result);
            Assert.Equal(201, createdAtActionResult.StatusCode);
            Assert.Equal(alertaViewModel, createdAtActionResult.Value);
        }


        [Fact]
        public void Post_DeveRetornar400_QuandoPontoColetaNaoExistir()
        {
            // Arrange
            var criarAlertaViewModel = new CriarAlertaViewModel { PontoColetaId = 1, Mensagem = "Teste" };
            var alertaModel = new AlertaModel { PontoColetaId = 1, Mensagem = "Teste" };
            _mapperMock.Setup(m => m.Map<AlertaModel>(criarAlertaViewModel)).Returns(alertaModel);
            _pontoColetaServiceMock.Setup(s => s.ObterPorId(1)).Returns((PontoColetaModel)null);

            // Act
            var resultado = _controller.Post(criarAlertaViewModel);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(resultado.Result);
            Assert.Equal("Ponto de Coleta não encontrado.", badRequest.Value);
        }

        [Fact]
        public void Post_DeveRetornar400_QuandoExcecaoForLancada()
        {
            // Arrange
            var criarAlertaViewModel = new CriarAlertaViewModel { PontoColetaId = 1, Mensagem = "Teste" };
            var alertaModel = new AlertaModel { PontoColetaId = 1, Mensagem = "Teste" };
            _mapperMock.Setup(m => m.Map<AlertaModel>(criarAlertaViewModel)).Returns(alertaModel);
            _pontoColetaServiceMock.Setup(s => s.ObterPorId(1)).Throws(new Exception("Erro"));

            // Act
            var resultado = _controller.Post(criarAlertaViewModel);

            // Assert
            Assert.IsType<BadRequestObjectResult>(resultado.Result);
        }

        [Fact]
        public void Put_DeveRetornar204_QuandoAtualizado()
        {
            // Arrange
            var atualizarAlertaViewModel = new AtualizarAlertaViewModel
            {
                Id = 1,
                PontoColetaId = 1,
                Mensagem = "Mensagem",
                Resolvido = false
            };
            var alertaExistente = new AlertaModel { Id = 1 };
            _alertaServiceMock.Setup(s => s.ObterPorId(1)).Returns(alertaExistente);

            // Act
            var resultado = _controller.Put(1, atualizarAlertaViewModel);

            // Assert
            Assert.IsType<NoContentResult>(resultado);
        }

        [Fact]
        public void Put_DeveRetornar400_QuandoIdDivergente()
        {
            // Arrange
            var atualizarAlertaViewModel = new AtualizarAlertaViewModel { Id = 2 };

            // Act
            var resultado = _controller.Put(1, atualizarAlertaViewModel);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(resultado);
            Assert.Equal("O id fornecido é divergente do alerta que será atualizado.", badRequest.Value);
        }

        [Fact]
        public void Put_DeveRetornar404_QuandoNaoEncontrado()
        {
            // Arrange
            var atualizarAlertaViewModel = new AtualizarAlertaViewModel { Id = 1 };
            _alertaServiceMock.Setup(s => s.ObterPorId(1)).Returns((AlertaModel)null);

            // Act
            var resultado = _controller.Put(1, atualizarAlertaViewModel);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(resultado);
            Assert.Equal("Alerta não encontrado.", notFound.Value);
        }

        [Fact]
        public void Put_DeveRetornar400_QuandoExcecaoForLancada()
        {
            // Arrange
            var atualizarAlertaViewModel = new AtualizarAlertaViewModel { Id = 1 };
            var alertaExistente = new AlertaModel { Id = 1 };
            _alertaServiceMock.Setup(s => s.ObterPorId(1)).Returns(alertaExistente);
            _alertaServiceMock.Setup(s => s.Atualizar(alertaExistente)).Throws(new Exception("Erro"));

            // Act
            var resultado = _controller.Put(1, atualizarAlertaViewModel);

            // Assert
            Assert.IsType<BadRequestObjectResult>(resultado);
        }

        [Fact]
        public void Delete_DeveRetornar204_QuandoDeletado()
        {
            // Arrange
            var alertaExistente = new AlertaModel { Id = 1 };
            _alertaServiceMock.Setup(s => s.ObterPorId(1)).Returns(alertaExistente);

            // Act
            var resultado = _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(resultado);
        }

        [Fact]
        public void Delete_DeveRetornar404_QuandoNaoEncontrado()
        {
            // Arrange
            _alertaServiceMock.Setup(s => s.ObterPorId(1)).Returns((AlertaModel)null);

            // Act
            var resultado = _controller.Delete(1);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(resultado);
            Assert.Equal("Alerta não encontrado.", notFound.Value);
        }

        [Fact]
        public void Delete_DeveRetornar400_QuandoExcecaoForLancada()
        {
            // Arrange
            var alertaExistente = new AlertaModel { Id = 1 };
            _alertaServiceMock.Setup(s => s.ObterPorId(1)).Returns(alertaExistente);
            _alertaServiceMock.Setup(s => s.Deletar(1)).Throws(new Exception("Erro"));

            // Act
            var resultado = _controller.Delete(1);

            // Assert
            Assert.IsType<BadRequestObjectResult>(resultado);
        }

        [Fact]
        public void GetPorPontoColeta_DeveRetornar200_QuandoEncontrado()
        {
            // Arrange
            var pontoColeta = new PontoColetaModel { Id = 1 };
            var alertas = new List<AlertaModel>();
            var alertasViewModel = new List<AlertaViewModel>();
            _pontoColetaServiceMock.Setup(s => s.ObterPorId(1)).Returns(pontoColeta);
            _alertaServiceMock.Setup(s => s.ListarPorPontoDeColeta(1, 0, 10)).Returns(alertas);
            _mapperMock.Setup(m => m.Map<IEnumerable<AlertaViewModel>>(alertas)).Returns(alertasViewModel);

            // Act
            var resultado = _controller.Get(1, 0, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            Assert.Equal(alertasViewModel, okResult.Value);
        }

        [Fact]
        public void GetPorPontoColeta_DeveRetornar404_QuandoPontoColetaNaoEncontrado()
        {
            // Arrange
            _pontoColetaServiceMock.Setup(s => s.ObterPorId(1)).Returns((PontoColetaModel)null);

            // Act
            var resultado = _controller.Get(1, 0, 10);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(resultado.Result);
            Assert.Equal("Ponto de coleta não encontrado.", notFound.Value);
        }

        [Fact]
        public void GetPorPontoColeta_DeveRetornar400_QuandoExcecaoForLancada()
        {
            // Arrange
            var pontoColeta = new PontoColetaModel { Id = 1 };
            _pontoColetaServiceMock.Setup(s => s.ObterPorId(1)).Returns(pontoColeta);
            _alertaServiceMock.Setup(s => s.ListarPorPontoDeColeta(1, 0, 10)).Throws(new Exception("Erro"));

            // Act
            var resultado = _controller.Get(1, 0, 10);

            // Assert
            Assert.IsType<BadRequestObjectResult>(resultado.Result);
        }
    }
}