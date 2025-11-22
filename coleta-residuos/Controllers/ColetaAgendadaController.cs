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
    public class ColetaAgendadaController : ControllerBase
    {
        private readonly IColetaAgendadaService _coletaAgendadaService;
        private readonly IService<PontoColetaModel> _pontoColetaService;
        private readonly IMapper _mapper;

        public ColetaAgendadaController(IColetaAgendadaService coletaAgendadaService, 
            IService<PontoColetaModel> pontoColetaModel, IMapper mapper)
        {
            _coletaAgendadaService = coletaAgendadaService;
            _pontoColetaService = pontoColetaModel;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ColetaAgendadaViewModel>> Get([FromQuery] int pagina = 0, [FromQuery] int tamanho = 10)
        {
            try
            {
            var coletas = _coletaAgendadaService.Listar(pagina, tamanho);

            var viewModels = _mapper.Map<IEnumerable<ColetaAgendadaViewModel>>(coletas);
            return Ok(viewModels);
        }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<ColetaAgendadaViewModel> Get(int id)
        {
            try
            {
            var coleta = _coletaAgendadaService.ObterPorId(id);
            if (coleta == null)
                return NotFound();
            var viewModel = _mapper.Map<ColetaAgendadaViewModel>(coleta);
            return Ok(viewModel);
        }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<ColetaAgendadaViewModel> Post([FromBody] CriarColetaAgendadaViewModel coletaViewModel)
        {
            var coleta = _mapper.Map<ColetaAgendadaModel>(coletaViewModel);
            _coletaAgendadaService.Criar(coleta);

            try
            {
        }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] AtualizarColetaAgendadaViewModel coletaViewModel)
        {
            if (id != coletaViewModel.Id)
                return BadRequest();

            try
            {
            var coletaExistente = _coletaAgendadaService.ObterPorId(id);
            if (coletaExistente == null)
                return NotFound();

            var coleta = _mapper.Map(coletaViewModel, coletaExistente);
            _coletaAgendadaService.Atualizar(coleta);
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
            var coleta = _coletaAgendadaService.ObterPorId(id);
            if (coleta == null)
                return NotFound();

            _coletaAgendadaService.Deletar(id);
            return NoContent();
        }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("/api/PontoColeta/{pontoColetaId}/ColetaAgendada")]
        public ActionResult<IEnumerable<ColetaAgendadaViewModel>> Get(int pontoColetaId, [FromQuery] int pagina = 0, [FromQuery] int tamanho = 10)
        {
            try
            {
            var pontoColeta = _pontoColetaService.ObterPorId(pontoColetaId);
            if (pontoColeta == null)
                return NotFound("Ponto de coleta não encontrado.");

            var coletas = _coletaAgendadaService.ListarPorPontoDeColeta(pontoColetaId, pagina, tamanho);

            var viewModels = _mapper.Map<IEnumerable<ColetaAgendadaViewModel>>(coletas);
            return Ok(viewModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
