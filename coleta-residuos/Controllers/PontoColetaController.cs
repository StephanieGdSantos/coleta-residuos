using AutoMapper;
using coleta_residuos.Models;
using coleta_residuos.Services;
using coleta_residuos.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace coleta_residuos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class PontoColetaController : ControllerBase
    {
        private readonly IService<PontoColetaModel> _pontoColetaService;
        private readonly IPontoColetaResiduoService _pontoColetaResiduoService;
        private readonly IMapper _mapper;

        public PontoColetaController(IService<PontoColetaModel> pontoColetaService, 
            IPontoColetaResiduoService pontoColetaResiduoService, IMapper mapper)
        {
            _pontoColetaService = pontoColetaService;
            _pontoColetaResiduoService = pontoColetaResiduoService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PontoColetaViewModel>> Get([FromQuery] int pagina = 0, 
            [FromQuery] int tamanho = 10)
        {
            try
            {
            var pontos = _pontoColetaService.Listar(pagina, tamanho);
            var pontosColetaViewModel = new List<PontoColetaViewModel>();
            var viewModels = _mapper.Map(pontos, pontosColetaViewModel);
            return Ok(viewModels);
        }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<PontoColetaViewModel> Get(int id)
        {
            try
            {
                return NotFound();

            var viewModel = _mapper.Map<PontoColetaViewModel>(ponto);
            return Ok(viewModel);
        }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}/Residuo")]
        public ActionResult<IEnumerable<ResiduoViewModel>> Get(int id,
            [FromQuery] int pagina = 0, [FromQuery] int tamanho = 10)
        {
            try
            {
                return NotFound("Ponto de oleta não encontrado.");

            var residuos = _pontoColetaResiduoService.ListarResiduosDoPontoDeColeta(id, pagina, tamanho);

            var viewModels = _mapper.Map<IEnumerable<ResiduoViewModel>>(residuos);
            return Ok(viewModels);
        }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<PontoColetaViewModel> Post([FromBody] CriarPontoColetaViewModel criarViewModel)
        {
            var ponto = _mapper.Map<PontoColetaModel>(criarViewModel);
            _pontoColetaService.Criar(ponto);

            try
            {
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] AtualizarPontoColetaViewModel atualizarViewModel)
        {
            if (id != atualizarViewModel.Id)
                return BadRequest();

            try
            {
            var pontoExistente = _pontoColetaService.ObterPorId(id);
            if (pontoExistente == null)
                return NotFound();

            _mapper.Map(atualizarViewModel, pontoExistente);
            _pontoColetaService.Atualizar(pontoExistente);

            return NoContent();
        }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
            var ponto = _pontoColetaService.ObterPorId(id);
            if (ponto == null)
                return NotFound();

            _pontoColetaService.Deletar(id);
            return NoContent();
        }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
