using Moq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using coleta_residuos.Controllers;
using coleta_residuos.Models;
using coleta_residuos.Services;
using coleta_residuos.ViewModel;

namespace coleta_residuos.Tests.Controllers
{
    public class ResiduoControllerTests
    {
        private readonly Mock<IService<ResiduoModel>> _residuoServiceMock;
        private readonly Mock<IPontoColetaResiduoService> _pontoColetaResiduoServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ResiduoController _controller;

        public ResiduoControllerTests()
        {
            _residuoServiceMock = new Mock<IService<ResiduoModel>>();
            _pontoColetaResiduoServiceMock = new Mock<IPontoColetaResiduoService>();
            _mapperMock = new Mock<IMapper>();

            _controller = new ResiduoController(_residuoServiceMock.Object,
                _pontoColetaResiduoServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public void Get_DeveRetornar200_ComListaDeResiduos()
        {
            // Arrange
            var residuos = new List<ResiduoModel>();
            var residuosViewModel = new List<ResiduoViewModel>();
            _residuoServiceMock.Setup(s => s.Listar(0, 10)).Returns(residuos);
            _mapperMock.Setup(m => m.Map<IEnumerable<ResiduoViewModel>>(residuos)).Returns(residuosViewModel);

            // Act
            var resultado = _controller.Get();

            // Assert
            Assert.IsType<OkObjectResult>(resultado.Result);
        }

        [Fact]
        public void Get_DeveRetornar400_QuandoException()
        {
            // Arrange
            _residuoServiceMock.Setup(s => s.Listar(0, 10)).Throws(new Exception("Erro"));

            // Act
            var resultado = _controller.Get();

            // Assert
            Assert.IsType<BadRequestObjectResult>(resultado.Result);
        }

        [Fact]
        public void GetPorId_DeveRetornar200_QuandoEncontrado()
        {
            // Arrange
            var residuo = new ResiduoModel { Id = 1 };
            var residuoViewModel = new ResiduoViewModel { Id = 1 };
            _residuoServiceMock.Setup(s => s.ObterPorId(1)).Returns(residuo);
            _mapperMock.Setup(m => m.Map<ResiduoViewModel>(residuo)).Returns(residuoViewModel);

            // Act
            var resultado = _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            Assert.Equal(residuoViewModel, okResult.Value);
        }

        [Fact]
        public void GetPorId_DeveRetornar404_QuandoNaoEncontrado()
        {
            // Arrange
            _residuoServiceMock.Setup(s => s.ObterPorId(1)).Returns((ResiduoModel)null);

            // Act
            var resultado = _controller.Get(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(resultado.Result);
            Assert.Equal("Resíduo não encontrado.", notFoundResult.Value);
        }

        [Fact]
        public void GetPorId_DeveRetornar400_QuandoException()
        {
            // Arrange
            _residuoServiceMock.Setup(s => s.ObterPorId(1)).Throws(new Exception("Erro"));

            // Act
            var resultado = _controller.Get(1);

            // Assert
            Assert.IsType<BadRequestObjectResult>(resultado.Result);
        }

        [Fact]
        public void GetPontosColeta_DeveRetornar200_QuandoEncontrado()
        {
            // Arrange
            var residuo = new ResiduoModel { Id = 1 };
            var pontos = new List<PontoColetaModel>();
            var pontosViewModel = new List<PontoColetaViewModel>();
            _residuoServiceMock.Setup(s => s.ObterPorId(1)).Returns(residuo);
            _pontoColetaResiduoServiceMock.Setup(s => s.ListarPontosDeColetaPorResiduo(1, 0, 10)).Returns(pontos);
            _mapperMock.Setup(m => m.Map<IEnumerable<PontoColetaViewModel>>(pontos)).Returns(pontosViewModel);

            // Act
            var resultado = _controller.Get(1, 0, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            Assert.Equal(pontosViewModel, okResult.Value);
        }

        [Fact]
        public void GetPontosColeta_DeveRetornar404_QuandoResiduoNaoEncontrado()
        {
            // Arrange
            _residuoServiceMock.Setup(s => s.ObterPorId(1)).Returns((ResiduoModel)null);

            // Act
            var resultado = _controller.Get(1, 0, 10);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(resultado.Result);
            Assert.Equal("Resíduo não encontrado.", notFoundResult.Value);
        }

        [Fact]
        public void GetPontosColeta_DeveRetornar400_QuandoException()
        {
            // Arrange
            var residuo = new ResiduoModel { Id = 1 };
            _residuoServiceMock.Setup(s => s.ObterPorId(1)).Returns(residuo);
            _pontoColetaResiduoServiceMock.Setup(s => s.ListarPontosDeColetaPorResiduo(1, 0, 10)).Throws(new Exception("Erro"));

            // Act
            var resultado = _controller.Get(1, 0, 10);

            // Assert
            Assert.IsType<BadRequestObjectResult>(resultado.Result);
        }

        [Fact]
        public void Post_DeveRetornar201_QuandoCriado()
        {
            // Arrange
            var criarViewModel = new CriarResiduoViewModel
            {
                Nome = "Resíduo",
                Categoria = "Categoria",
                Descricao = "Descrição"
            };
            var residuoModel = new ResiduoModel { Id = 1 };
            _mapperMock.Setup(m => m.Map<ResiduoModel>(criarViewModel)).Returns(residuoModel);

            // Act
            var resultado = _controller.Post(criarViewModel);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(resultado.Result);
            Assert.Equal(criarViewModel, createdResult.Value);
        }

        [Fact]
        public void Post_DeveRetornar400_QuandoException()
        {
            // Arrange
            var criarViewModel = new CriarResiduoViewModel
            {
                Nome = "Resíduo",
                Categoria = "Categoria",
                Descricao = "Descrição"
            };
            var residuoModel = new ResiduoModel { Id = 1 };
            _mapperMock.Setup(m => m.Map<ResiduoModel>(criarViewModel)).Returns(residuoModel);
            _residuoServiceMock.Setup(s => s.Criar(residuoModel)).Throws(new Exception("Erro"));

            // Act
            var resultado = _controller.Post(criarViewModel);

            // Assert
            Assert.IsType<BadRequestObjectResult>(resultado.Result);
        }

        [Fact]
        public void Put_DeveRetornar204_QuandoAtualizado()
        {
            // Arrange
            var atualizarViewModel = new AtualizarResiduoViewModel
            {
                Id = "1",
                Nome = "Resíduo",
                Categoria = "Categoria",
                Descricao = "Descrição"
            };
            var residuoExistente = new ResiduoModel { Id = 1 };
            _residuoServiceMock.Setup(s => s.ObterPorId(1)).Returns(residuoExistente);

            // Act
            var resultado = _controller.Put(1, atualizarViewModel);

            // Assert
            Assert.IsType<NoContentResult>(resultado);
        }

        [Fact]
        public void Put_DeveRetornar400_QuandoIdDivergente()
        {
            // Arrange
            var atualizarViewModel = new AtualizarResiduoViewModel { Id = "2" };

            // Act
            var resultado = _controller.Put(1, atualizarViewModel);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(resultado);
            Assert.Equal("O id fornecido é divergente do resíduo que será atualizado.", badRequest.Value);
        }

        [Fact]
        public void Put_DeveRetornar404_QuandoNaoEncontrado()
        {
            // Arrange
            var atualizarViewModel = new AtualizarResiduoViewModel { Id = "1" };
            _residuoServiceMock.Setup(s => s.ObterPorId(1)).Returns((ResiduoModel)null);

            // Act
            var resultado = _controller.Put(1, atualizarViewModel);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(resultado);
            Assert.Equal("Resíduo não encontrado", notFound.Value);
        }

        [Fact]
        public void Put_DeveRetornar400_QuandoException()
        {
            // Arrange
            var atualizarViewModel = new AtualizarResiduoViewModel { Id = "1" };
            var residuoExistente = new ResiduoModel { Id = 1 };
            _residuoServiceMock.Setup(s => s.ObterPorId(1)).Returns(residuoExistente);
            _residuoServiceMock.Setup(s => s.Atualizar(residuoExistente)).Throws(new Exception("Erro"));

            // Act
            var resultado = _controller.Put(1, atualizarViewModel);

            // Assert
            Assert.IsType<BadRequestObjectResult>(resultado);
        }

        [Fact]
        public void Delete_DeveRetornar204_QuandoDeletado()
        {
            // Arrange
            var residuoExistente = new ResiduoModel { Id = 1 };
            _residuoServiceMock.Setup(s => s.ObterPorId(1)).Returns(residuoExistente);

            // Act
            var resultado = _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(resultado);
        }

        [Fact]
        public void Delete_DeveRetornar404_QuandoNaoEncontrado()
        {
            // Arrange
            _residuoServiceMock.Setup(s => s.ObterPorId(1)).Returns((ResiduoModel)null);

            // Act
            var resultado = _controller.Delete(1);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(resultado);
            Assert.Equal("Resíduo não encontrado.", notFound.Value);
        }

        [Fact]
        public void Delete_DeveRetornar400_QuandoException()
        {
            // Arrange
            var residuoExistente = new ResiduoModel { Id = 1 };
            _residuoServiceMock.Setup(s => s.ObterPorId(1)).Returns(residuoExistente);
            _residuoServiceMock.Setup(s => s.Deletar(1)).Throws(new Exception("Erro"));

            // Act
            var resultado = _controller.Delete(1);

            // Assert
            Assert.IsType<BadRequestObjectResult>(resultado);
        }
    }
}