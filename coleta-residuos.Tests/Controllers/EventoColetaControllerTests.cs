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
    public class EventoColetaControllerTests
    {
        private readonly Mock<IEventoColetaService> _eventoColetaServiceMock;
        private readonly Mock<IService<PontoColetaModel>> _pontoColetaServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly EventoColetaController _controller;

        public EventoColetaControllerTests()
        {
            _eventoColetaServiceMock = new Mock<IEventoColetaService>();
            _pontoColetaServiceMock = new Mock<IService<PontoColetaModel>>();
            _mapperMock = new Mock<IMapper>();

            _controller = new EventoColetaController(_eventoColetaServiceMock.Object,
                _pontoColetaServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public void Get_DeveRetornar200_ComListaDeColetas()
        {
            // Arrange
            _eventoColetaServiceMock.Setup(s => s.Listar(0, 10)).Returns(new List<EventoColetaModel>());
            _mapperMock.Setup(m => m.Map<IEnumerable<EventoColetaViewModel>>(It.IsAny<IEnumerable<EventoColetaModel>>()))
                .Returns(new List<EventoColetaViewModel>());

            // Act
            var resultado = _controller.Get();

            // Assert
            Assert.IsType<OkObjectResult>(resultado.Result);
        }

        [Fact]
        public void Get_DeveRetornar400_QuandoException()
        {
            // Arrange
            _eventoColetaServiceMock.Setup(s => s.Listar(0, 10)).Throws(new Exception("Erro"));

            // Act
            var resultado = _controller.Get();

            // Assert
            Assert.IsType<BadRequestObjectResult>(resultado.Result);
        }

        [Fact]
        public void GetPorId_DeveRetornar200_QuandoEncontrado()
        {
            // Arrange
            var evento = new EventoColetaModel { Id = 1 };
            var eventoViewModel = new EventoColetaViewModel { Id = 1 };
            _eventoColetaServiceMock.Setup(s => s.ObterPorId(1)).Returns(evento);
            _mapperMock.Setup(m => m.Map<EventoColetaViewModel>(evento)).Returns(eventoViewModel);

            // Act
            var resultado = _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            Assert.Equal(eventoViewModel, okResult.Value);
        }

        [Fact]
        public void GetPorId_DeveRetornar404_QuandoNaoEncontrado()
        {
            // Arrange
            _eventoColetaServiceMock.Setup(s => s.ObterPorId(1)).Returns((EventoColetaModel)null);

            // Act
            var resultado = _controller.Get(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(resultado.Result);
            Assert.Equal("Evento não encontrado.", notFoundResult.Value);
        }

        [Fact]
        public void GetPorId_DeveRetornar400_QuandoException()
        {
            // Arrange
            _eventoColetaServiceMock.Setup(s => s.ObterPorId(1)).Throws(new Exception("Erro"));

            // Act
            var resultado = _controller.Get(1);

            // Assert
            Assert.IsType<BadRequestObjectResult>(resultado.Result);
        }

        [Fact]
        public void Post_DeveRetornar201_QuandoCriado()
        {
            // Arrange
            var criarViewModel = new CriarEventoColetaViewModel
            {
                PontoColetaId = 1,
                ResiduoId = 1,
                DataEvento = DateTime.Now,
                QuantidadeKg = 10,
                TipoEvento = TipoEvento.Deposito
            };
            var eventoModel = new EventoColetaModel { Id = 1 };
            var eventoViewModel = new EventoColetaViewModel { Id = 1 };
            _mapperMock.Setup(m => m.Map<EventoColetaModel>(criarViewModel)).Returns(eventoModel);
            _mapperMock.Setup(m => m.Map<EventoColetaViewModel>(eventoModel)).Returns(eventoViewModel);

            // Act
            var resultado = _controller.Post(criarViewModel);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(resultado.Result);
            Assert.Equal(eventoViewModel, createdResult.Value);
        }


        [Fact]
        public void Post_DeveRetornar400_QuandoException()
        {
            // Arrange
            var criarViewModel = new CriarEventoColetaViewModel();
            var eventoModel = new EventoColetaModel();
            _mapperMock.Setup(m => m.Map<EventoColetaModel>(criarViewModel)).Returns(eventoModel);
            _eventoColetaServiceMock.Setup(s => s.Criar(eventoModel)).Throws(new Exception("Erro"));

            // Act
            var resultado = _controller.Post(criarViewModel);

            // Assert
            Assert.IsType<BadRequestObjectResult>(resultado.Result);
        }

        [Fact]
        public void Put_DeveRetornar204_QuandoAtualizado()
        {
            // Arrange
            var atualizarViewModel = new AtualizarEventoColetaViewModel
            {
                Id = 1,
                PontoColetaId = 1,
                ResiduoId = 1,
                DataEvento = DateTime.Now,
                QuantidadeKg = 10,
                TipoEvento = TipoEvento.Deposito
            };
            var eventoExistente = new EventoColetaModel { Id = 1 };
            _eventoColetaServiceMock.Setup(s => s.ObterPorId(1)).Returns(eventoExistente);

            // Act
            var resultado = _controller.Put(1, atualizarViewModel);

            // Assert
            Assert.IsType<NoContentResult>(resultado);
        }

        [Fact]
        public void Put_DeveRetornar400_QuandoIdDivergente()
        {
            // Arrange
            var atualizarViewModel = new AtualizarEventoColetaViewModel { Id = 2 };

            // Act
            var resultado = _controller.Put(1, atualizarViewModel);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(resultado);
            Assert.Equal("O id fornecido é divergente do evento que será atualizado.", badRequest.Value);
        }

        [Fact]
        public void Put_DeveRetornar404_QuandoNaoEncontrado()
        {
            // Arrange
            var atualizarViewModel = new AtualizarEventoColetaViewModel { Id = 1 };
            _eventoColetaServiceMock.Setup(s => s.ObterPorId(1)).Returns((EventoColetaModel)null);

            // Act
            var resultado = _controller.Put(1, atualizarViewModel);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(resultado);
            Assert.Equal("Evento não encontrado.", notFound.Value);
        }

        [Fact]
        public void Put_DeveRetornar400_QuandoException()
        {
            // Arrange
            var atualizarViewModel = new AtualizarEventoColetaViewModel { Id = 1 };
            var eventoExistente = new EventoColetaModel { Id = 1 };
            _eventoColetaServiceMock.Setup(s => s.ObterPorId(1)).Returns(eventoExistente);
            _eventoColetaServiceMock.Setup(s => s.Atualizar(eventoExistente)).Throws(new Exception("Erro"));

            // Act
            var resultado = _controller.Put(1, atualizarViewModel);

            // Assert
            Assert.IsType<BadRequestObjectResult>(resultado);
        }

        [Fact]
        public void Delete_DeveRetornar204_QuandoDeletado()
        {
            // Arrange
            var eventoExistente = new EventoColetaModel { Id = 1 };
            _eventoColetaServiceMock.Setup(s => s.ObterPorId(1)).Returns(eventoExistente);

            // Act
            var resultado = _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(resultado);
        }

        [Fact]
        public void Delete_DeveRetornar404_QuandoNaoEncontrado()
        {
            // Arrange
            _eventoColetaServiceMock.Setup(s => s.ObterPorId(1)).Returns((EventoColetaModel)null);

            // Act
            var resultado = _controller.Delete(1);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(resultado);
            Assert.Equal("Evento não encontrado.", notFound.Value);
        }

        [Fact]
        public void Delete_DeveRetornar400_QuandoException()
        {
            // Arrange
            var eventoExistente = new EventoColetaModel { Id = 1 };
            _eventoColetaServiceMock.Setup(s => s.ObterPorId(1)).Returns(eventoExistente);
            _eventoColetaServiceMock.Setup(s => s.Deletar(1)).Throws(new Exception("Erro"));

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
            var eventos = new List<EventoColetaModel>();
            var eventosViewModel = new List<EventoColetaViewModel>();
            _pontoColetaServiceMock.Setup(s => s.ObterPorId(1)).Returns(pontoColeta);
            _eventoColetaServiceMock.Setup(s => s.ListarPorPontoDeColeta(1, 0, 10)).Returns(eventos);
            _mapperMock.Setup(m => m.Map<IEnumerable<EventoColetaViewModel>>(eventos)).Returns(eventosViewModel);

            // Act
            var resultado = _controller.Get(1, 0, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            Assert.Equal(eventosViewModel, okResult.Value);
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
        public void GetPorPontoColeta_DeveRetornar400_QuandoException()
        {
            // Arrange
            var pontoColeta = new PontoColetaModel { Id = 1 };
            _pontoColetaServiceMock.Setup(s => s.ObterPorId(1)).Returns(pontoColeta);
            _eventoColetaServiceMock.Setup(s => s.ListarPorPontoDeColeta(1, 0, 10)).Throws(new Exception("Erro"));

            // Act
            var resultado = _controller.Get(1, 0, 10);

            // Assert
            Assert.IsType<BadRequestObjectResult>(resultado.Result);
        }
    }
}
