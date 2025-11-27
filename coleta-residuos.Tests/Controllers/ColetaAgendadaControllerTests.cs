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
    public class ColetaAgendadaControllerTests
    {
        private readonly Mock<IColetaAgendadaService> _coletaAgendadaServiceMock;
        private readonly Mock<IService<PontoColetaModel>> _pontoColetaServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ColetaAgendadaController _controller;

        public ColetaAgendadaControllerTests()
        {
            _coletaAgendadaServiceMock = new Mock<IColetaAgendadaService>();
            _pontoColetaServiceMock = new Mock<IService<PontoColetaModel>>();
            _mapperMock = new Mock<IMapper>();

            _controller = new ColetaAgendadaController(_coletaAgendadaServiceMock.Object,
                _pontoColetaServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public void Get_DeveRetornar200_ComListaDeColetasAgendadas()
        {
            // Arrange
            var coletas = new List<ColetaAgendadaModel>();
            var coletasViewModel = new List<ColetaAgendadaViewModel>();
            _coletaAgendadaServiceMock.Setup(s => s.Listar(0, 10)).Returns(coletas);
            _mapperMock.Setup(m => m.Map<IEnumerable<ColetaAgendadaViewModel>>(coletas)).Returns(coletasViewModel);

            // Act
            var resultado = _controller.Get();

            // Assert
            Assert.IsType<OkObjectResult>(resultado.Result);
        }

        [Fact]
        public void Get_DeveRetornar500_QuandoExcecaoForLancada()
        {
            // Arrange
            _coletaAgendadaServiceMock.Setup(s => s.Listar(0, 10)).Throws(new Exception("Erro"));

            // Act
            var resultado = _controller.Get();

            // Assert
            Assert.IsType<ObjectResult>(resultado.Result);
            var objectResult = resultado.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public void GetPorId_DeveRetornar200_ComColetaAgendadaExistente()
        {
            // Arrange
            var coleta = new ColetaAgendadaModel { Id = 1 };
            var coletaViewModel = new ColetaAgendadaViewModel { Id = 1 };
            _coletaAgendadaServiceMock.Setup(s => s.ObterPorId(1)).Returns(coleta);
            _mapperMock.Setup(m => m.Map<ColetaAgendadaViewModel>(coleta)).Returns(coletaViewModel);

            // Act
            var resultado = _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            Assert.Equal(coletaViewModel, okResult.Value);
        }

        [Fact]
        public void GetPorId_DeveRetornar404_QuandoColetaAgendadaNaoExistir()
        {
            // Arrange
            _coletaAgendadaServiceMock.Setup(s => s.ObterPorId(1)).Returns((ColetaAgendadaModel)null);

            // Act
            var resultado = _controller.Get(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(resultado.Result);
            Assert.Equal("Agendamento não encontrado.", notFoundResult.Value);
        }

        [Fact]
        public void GetPorId_DeveRetornar400_QuandoExcecaoForLancada()
        {
            // Arrange
            _coletaAgendadaServiceMock.Setup(s => s.ObterPorId(1)).Throws(new Exception("Erro"));

            // Act
            var resultado = _controller.Get(1);

            // Assert
            Assert.IsType<BadRequestObjectResult>(resultado.Result);
        }

        [Fact]
        public void Post_DeveRetornar201_QuandoCriado()
        {
            // Arrange
            var criarViewModel = new CriarColetaAgendadaViewModel
            {
                DataAgendada = DateTime.Now,
                Observacao = "Teste",
                PontoColetaId = 1
            };
            var coletaModel = new ColetaAgendadaModel { Id = 1 };
            _mapperMock.Setup(m => m.Map<ColetaAgendadaModel>(criarViewModel)).Returns(coletaModel);

            // Act
            var resultado = _controller.Post(criarViewModel);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(resultado.Result);
            Assert.Equal(criarViewModel, createdResult.Value);
        }

        [Fact]
        public void Post_DeveRetornar400_QuandoExcecaoForLancada()
        {
            // Arrange
            var criarViewModel = new CriarColetaAgendadaViewModel
            {
                DataAgendada = DateTime.Now,
                Observacao = "Teste",
                PontoColetaId = 1
            };
            var coletaModel = new ColetaAgendadaModel { Id = 1 };
            _mapperMock.Setup(m => m.Map<ColetaAgendadaModel>(criarViewModel)).Returns(coletaModel);
            _coletaAgendadaServiceMock.Setup(s => s.Criar(coletaModel)).Throws(new Exception("Erro"));

            // Act
            var resultado = _controller.Post(criarViewModel);

            // Assert
            Assert.IsType<BadRequestObjectResult>(resultado.Result);
        }

        [Fact]
        public void Put_DeveRetornar204_QuandoAtualizado()
        {
            // Arrange
            var atualizarViewModel = new AtualizarColetaAgendadaViewModel
            {
                Id = 1,
                DataAgendada = DateTime.Now,
                Observacao = "Teste",
                PontoColetaId = 1
            };
            var coletaExistente = new ColetaAgendadaModel { Id = 1 };
            _coletaAgendadaServiceMock.Setup(s => s.ObterPorId(1)).Returns(coletaExistente);

            // Act
            var resultado = _controller.Put(1, atualizarViewModel);

            // Assert
            Assert.IsType<NoContentResult>(resultado);
        }

        [Fact]
        public void Put_DeveRetornar400_QuandoIdDivergente()
        {
            // Arrange
            var atualizarViewModel = new AtualizarColetaAgendadaViewModel { Id = 2 };

            // Act
            var resultado = _controller.Put(1, atualizarViewModel);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(resultado);
            Assert.Equal("O id fornecido é divergente do agendamento que será atualizado.", badRequest.Value);
        }

        [Fact]
        public void Put_DeveRetornar404_QuandoNaoEncontrado()
        {
            // Arrange
            var atualizarViewModel = new AtualizarColetaAgendadaViewModel { Id = 1 };
            _coletaAgendadaServiceMock.Setup(s => s.ObterPorId(1)).Returns((ColetaAgendadaModel)null);

            // Act
            var resultado = _controller.Put(1, atualizarViewModel);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(resultado);
            Assert.Equal("Agendamento não encontrado.", notFound.Value);
        }

        [Fact]
        public void Put_DeveRetornar400_QuandoExcecaoForLancada()
        {
            // Arrange
            var atualizarViewModel = new AtualizarColetaAgendadaViewModel { Id = 1 };
            var coletaExistente = new ColetaAgendadaModel { Id = 1 };
            _coletaAgendadaServiceMock.Setup(s => s.ObterPorId(1)).Returns(coletaExistente);
            _coletaAgendadaServiceMock.Setup(s => s.Atualizar(coletaExistente)).Throws(new Exception("Erro"));

            // Act
            var resultado = _controller.Put(1, atualizarViewModel);

            // Assert
            Assert.IsType<BadRequestObjectResult>(resultado);
        }

        [Fact]
        public void Delete_DeveRetornar204_QuandoDeletado()
        {
            // Arrange
            var coletaExistente = new ColetaAgendadaModel { Id = 1 };
            _coletaAgendadaServiceMock.Setup(s => s.ObterPorId(1)).Returns(coletaExistente);

            // Act
            var resultado = _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(resultado);
        }

        [Fact]
        public void Delete_DeveRetornar404_QuandoNaoEncontrado()
        {
            // Arrange
            _coletaAgendadaServiceMock.Setup(s => s.ObterPorId(1)).Returns((ColetaAgendadaModel)null);

            // Act
            var resultado = _controller.Delete(1);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(resultado);
            Assert.Equal("Agendamento não encontrado.", notFound.Value);
        }

        [Fact]
        public void Delete_DeveRetornar400_QuandoExcecaoForLancada()
        {
            // Arrange
            var coletaExistente = new ColetaAgendadaModel { Id = 1 };
            _coletaAgendadaServiceMock.Setup(s => s.ObterPorId(1)).Returns(coletaExistente);
            _coletaAgendadaServiceMock.Setup(s => s.Deletar(1)).Throws(new Exception("Erro"));

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
            var coletas = new List<ColetaAgendadaModel>();
            var coletasViewModel = new List<ColetaAgendadaViewModel>();
            _pontoColetaServiceMock.Setup(s => s.ObterPorId(1)).Returns(pontoColeta);
            _coletaAgendadaServiceMock.Setup(s => s.ListarPorPontoDeColeta(1, 0, 10)).Returns(coletas);
            _mapperMock.Setup(m => m.Map<IEnumerable<ColetaAgendadaViewModel>>(coletas)).Returns(coletasViewModel);

            // Act
            var resultado = _controller.Get(1, 0, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            Assert.Equal(coletasViewModel, okResult.Value);
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
            _coletaAgendadaServiceMock.Setup(s => s.ListarPorPontoDeColeta(1, 0, 10)).Throws(new Exception("Erro"));

            // Act
            var resultado = _controller.Get(1, 0, 10);

            // Assert
            Assert.IsType<BadRequestObjectResult>(resultado.Result);
        }
    }
}