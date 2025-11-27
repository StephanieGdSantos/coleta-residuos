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
using System.Linq;

namespace coleta_residuos.Tests.Controllers
{
    public class PontoColetaControllerTests
    {
        private readonly Mock<IService<PontoColetaModel>> _pontoColetaServiceMock;
        private readonly Mock<IPontoColetaResiduoService> _pontoColetaResiduoServiceMock;
        private readonly Mock<IService<ResiduoModel>> _residuoServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly PontoColetaController _controller;

        public PontoColetaControllerTests()
        {
            _pontoColetaServiceMock = new Mock<IService<PontoColetaModel>>();
            _pontoColetaResiduoServiceMock = new Mock<IPontoColetaResiduoService>();
            _residuoServiceMock = new Mock<IService<ResiduoModel>>();
            _mapperMock = new Mock<IMapper>();

            _controller = new PontoColetaController(_pontoColetaServiceMock.Object,
                _pontoColetaResiduoServiceMock.Object, _residuoServiceMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public void Get_DeveRetornar200_ComListaDePontosDeColeta()
        {
            // Arrange
            var pontos = new List<PontoColetaModel>();
            var pontosViewModel = new List<PontoColetaViewModel>();
            _pontoColetaServiceMock.Setup(s => s.Listar(0, 10)).Returns(pontos);
            _mapperMock.Setup(m => m.Map<IEnumerable<PontoColetaViewModel>>(pontos)).Returns(pontosViewModel);

            // Act
            var resultado = _controller.Get();

            // Assert
            Assert.IsType<OkObjectResult>(resultado.Result);
        }

        [Fact]
        public void Get_DeveRetornar400_QuandoException()
        {
            // Arrange
            _pontoColetaServiceMock.Setup(s => s.Listar(0, 10)).Throws(new Exception("Erro"));

            // Act
            var resultado = _controller.Get();

            // Assert
            Assert.IsType<BadRequestObjectResult>(resultado.Result);
        }

        [Fact]
        public void GetPorId_DeveRetornar200_QuandoEncontrado()
        {
            // Arrange
            var ponto = new PontoColetaModel { Id = 1 };
            var pontoViewModel = new PontoColetaViewModel { Id = 1 };
            _pontoColetaServiceMock.Setup(s => s.ObterPorId(1)).Returns(ponto);
            _mapperMock.Setup(m => m.Map<PontoColetaViewModel>(ponto)).Returns(pontoViewModel);

            // Act
            var resultado = _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            Assert.Equal(pontoViewModel, okResult.Value);
        }

        [Fact]
        public void GetPorId_DeveRetornar404_QuandoNaoEncontrado()
        {
            // Arrange
            _pontoColetaServiceMock.Setup(s => s.ObterPorId(1)).Returns((PontoColetaModel)null);

            // Act
            var resultado = _controller.Get(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(resultado.Result);
            Assert.Equal("Ponto de coleta não encontrado.", notFoundResult.Value);
        }

        [Fact]
        public void GetPorId_DeveRetornar400_QuandoException()
        {
            // Arrange
            _pontoColetaServiceMock.Setup(s => s.ObterPorId(1)).Throws(new Exception("Erro"));

            // Act
            var resultado = _controller.Get(1);

            // Assert
            Assert.IsType<BadRequestObjectResult>(resultado.Result);
        }

        [Fact]
        public void GetResiduos_DeveRetornar200_QuandoEncontrado()
        {
            // Arrange
            var ponto = new PontoColetaModel { Id = 1 };
            var residuos = new List<ResiduoModel>();
            var residuosViewModel = new List<ResiduoViewModel>();
            _pontoColetaServiceMock.Setup(s => s.ObterPorId(1)).Returns(ponto);
            _pontoColetaResiduoServiceMock.Setup(s => s.ListarResiduosDoPontoDeColeta(1, 0, 10)).Returns(residuos);
            _mapperMock.Setup(m => m.Map<IEnumerable<ResiduoViewModel>>(residuos)).Returns(residuosViewModel);

            // Act
            var resultado = _controller.Get(1, 0, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            Assert.Equal(residuosViewModel, okResult.Value);
        }

        [Fact]
        public void GetResiduos_DeveRetornar404_QuandoPontoColetaNaoEncontrado()
        {
            // Arrange
            _pontoColetaServiceMock.Setup(s => s.ObterPorId(1)).Returns((PontoColetaModel)null);

            // Act
            var resultado = _controller.Get(1, 0, 10);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(resultado.Result);
            Assert.Equal("Ponto de coleta não encontrado.", notFoundResult.Value);
        }

        [Fact]
        public void GetResiduos_DeveRetornar400_QuandoException()
        {
            // Arrange
            var ponto = new PontoColetaModel { Id = 1 };
            _pontoColetaServiceMock.Setup(s => s.ObterPorId(1)).Returns(ponto);
            _pontoColetaResiduoServiceMock.Setup(s => s.ListarResiduosDoPontoDeColeta(1, 0, 10)).Throws(new Exception("Erro"));

            // Act
            var resultado = _controller.Get(1, 0, 10);

            // Assert
            Assert.IsType<BadRequestObjectResult>(resultado.Result);
        }

        [Fact]
        public void Post_DeveRetornar201_QuandoCriado()
        {
            // Arrange
            var criarViewModel = new CriarPontoColetaViewModel
            {
                Nome = "Ponto",
                Endereco = "Endereço",
                CapacidadeMaximaKg = 100,
                ResiduosIds = new List<int> { 1, 2 }
            };
            var pontoModel = new PontoColetaModel { Id = 1 };
            var residuos = new List<ResiduoModel>
            {
                new ResiduoModel { Id = 1 },
                new ResiduoModel { Id = 2 }
            };
            _mapperMock.Setup(m => m.Map<PontoColetaModel>(criarViewModel)).Returns(pontoModel);
            _residuoServiceMock.Setup(s => s.Listar(0, 10)).Returns(residuos);

            // Act
            var resultado = _controller.Post(criarViewModel);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(resultado.Result);
            Assert.Equal(criarViewModel, createdResult.Value);
        }

        [Fact]
        public void Post_DeveRetornar400_QuandoResiduosNaoExistem()
        {
            // Arrange
            var criarViewModel = new CriarPontoColetaViewModel
            {
                Nome = "Ponto",
                Endereco = "Endereço",
                CapacidadeMaximaKg = 100,
                ResiduosIds = new List<int> { 1, 2 }
            };
            var pontoModel = new PontoColetaModel { Id = 1 };
            var residuos = new List<ResiduoModel>
            {
                new ResiduoModel { Id = 1 }
            };
            _mapperMock.Setup(m => m.Map<PontoColetaModel>(criarViewModel)).Returns(pontoModel);
            _residuoServiceMock.Setup(s => s.Listar(0, 10)).Returns(residuos);

            // Act
            var resultado = _controller.Post(criarViewModel);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(resultado.Result);
            Assert.Contains("Os seguintes resíduos não existem", badRequest.Value.ToString());
        }

        [Fact]
        public void Post_DeveRetornar400_QuandoException()
        {
            // Arrange
            var criarViewModel = new CriarPontoColetaViewModel
            {
                Nome = "Ponto",
                Endereco = "Endereço",
                CapacidadeMaximaKg = 100,
                ResiduosIds = new List<int> { 1 }
            };
            var pontoModel = new PontoColetaModel { Id = 1 };
            var residuos = new List<ResiduoModel>
            {
                new ResiduoModel { Id = 1 }
            };
            _mapperMock.Setup(m => m.Map<PontoColetaModel>(criarViewModel)).Returns(pontoModel);
            _residuoServiceMock.Setup(s => s.Listar(0, 10)).Returns(residuos);
            _pontoColetaServiceMock.Setup(s => s.Criar(pontoModel)).Throws(new Exception("Erro"));

            // Act
            var resultado = _controller.Post(criarViewModel);

            // Assert
            Assert.IsType<BadRequestObjectResult>(resultado.Result);
        }

        [Fact]
        public void Put_DeveRetornar204_QuandoAtualizado()
        {
            // Arrange
            var atualizarViewModel = new AtualizarPontoColetaViewModel
            {
                Id = 1,
                Nome = "Ponto",
                Endereco = "Endereço",
                CapacidadeMaximaKg = 100
            };
            var pontoExistente = new PontoColetaModel { Id = 1 };
            _pontoColetaServiceMock.Setup(s => s.ObterPorId(1)).Returns(pontoExistente);

            // Act
            var resultado = _controller.Put(1, atualizarViewModel);

            // Assert
            Assert.IsType<NoContentResult>(resultado);
        }

        [Fact]
        public void Put_DeveRetornar400_QuandoIdDivergente()
        {
            // Arrange
            var atualizarViewModel = new AtualizarPontoColetaViewModel { Id = 2 };

            // Act
            var resultado = _controller.Put(1, atualizarViewModel);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(resultado);
            Assert.Equal("O id fornecido é divergente do ponto de coleta que será atualizado.", badRequest.Value);
        }

        [Fact]
        public void Put_DeveRetornar404_QuandoNaoEncontrado()
        {
            // Arrange
            var atualizarViewModel = new AtualizarPontoColetaViewModel { Id = 1 };
            _pontoColetaServiceMock.Setup(s => s.ObterPorId(1)).Returns((PontoColetaModel)null);

            // Act
            var resultado = _controller.Put(1, atualizarViewModel);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(resultado);
            Assert.Equal("Ponto de coleta não encontrado.", notFound.Value);
        }

        [Fact]
        public void Put_DeveRetornar400_QuandoException()
        {
            // Arrange
            var atualizarViewModel = new AtualizarPontoColetaViewModel { Id = 1 };
            var pontoExistente = new PontoColetaModel { Id = 1 };
            _pontoColetaServiceMock.Setup(s => s.ObterPorId(1)).Returns(pontoExistente);
            _pontoColetaServiceMock.Setup(s => s.Atualizar(pontoExistente)).Throws(new Exception("Erro"));

            // Act
            var resultado = _controller.Put(1, atualizarViewModel);

            // Assert
            Assert.IsType<BadRequestObjectResult>(resultado);
        }

        [Fact]
        public void Delete_DeveRetornar204_QuandoDeletado()
        {
            // Arrange
            var pontoExistente = new PontoColetaModel { Id = 1 };
            _pontoColetaServiceMock.Setup(s => s.ObterPorId(1)).Returns(pontoExistente);

            // Act
            var resultado = _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(resultado);
        }

        [Fact]
        public void Delete_DeveRetornar404_QuandoNaoEncontrado()
        {
            // Arrange
            _pontoColetaServiceMock.Setup(s => s.ObterPorId(1)).Returns((PontoColetaModel)null);

            // Act
            var resultado = _controller.Delete(1);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(resultado);
            Assert.Equal("Ponto de coleta não encontrado.", notFound.Value);
        }

        [Fact]
        public void Delete_DeveRetornar400_QuandoException()
        {
            // Arrange
            var pontoExistente = new PontoColetaModel { Id = 1 };
            _pontoColetaServiceMock.Setup(s => s.ObterPorId(1)).Returns(pontoExistente);
            _pontoColetaServiceMock.Setup(s => s.Deletar(1)).Throws(new Exception("Erro"));

            // Act
            var resultado = _controller.Delete(1);

            // Assert
            Assert.IsType<BadRequestObjectResult>(resultado);
        }
    }
}