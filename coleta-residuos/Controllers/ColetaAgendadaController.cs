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
        private readonly IMapper _mapper;

        public ColetaAgendadaController(IColetaAgendadaService coletaAgendadaService, IMapper mapper)
        {
            _coletaAgendadaService = coletaAgendadaService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ColetaAgendadaViewModel>> Get([FromQuery] int pagina = 0, [FromQuery] int tamanho = 10)
        {
            var coletas = _coletaAgendadaService.Listar(pagina, tamanho);
            var viewModels = _mapper.Map<IEnumerable<ColetaAgendadaViewModel>>(coletas);
            return Ok(viewModels);
        }

        [HttpGet("{id}")]
        public ActionResult<ColetaAgendadaViewModel> Get(int id)
        {
            var coleta = _coletaAgendadaService.ObterPorId(id);
            if (coleta == null)
                return NotFound();
            var viewModel = _mapper.Map<ColetaAgendadaViewModel>(coleta);
            return Ok(viewModel);
        }

        [HttpPost]
        public ActionResult<ColetaAgendadaViewModel> Post([FromBody] CriarColetaAgendadaViewModel coletaViewModel)
        {
            var coleta = _mapper.Map<ColetaAgendadaModel>(coletaViewModel);
            _coletaAgendadaService.Criar(coleta);

            var viewModel = _mapper.Map<ColetaAgendadaViewModel>(coleta);
            return CreatedAtAction(nameof(Post), new { id = coleta.Id }, viewModel);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] AtualizarColetaAgendadaViewModel coletaViewModel)
        {
            var coletaExistente = _coletaAgendadaService.ObterPorId(id);
            if (coletaExistente == null)
                return NotFound();

            var coleta = _mapper.Map<ColetaAgendadaModel>(coletaViewModel);
            _coletaAgendadaService.Atualizar(coleta);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var coleta = _coletaAgendadaService.ObterPorId(id);
            if (coleta == null)
                return NotFound();

            _coletaAgendadaService.Deletar(id);
            return NoContent();
        }

        [HttpGet("ponto-coleta/{pontoColetaId}")]
        public ActionResult<IEnumerable<ColetaAgendadaViewModel>> Get(int pontoColetaId, [FromQuery] int pagina = 0, [FromQuery] int tamanho = 10)
        {
            var coletas = _coletaAgendadaService.ListarPorPontoDeColeta(pontoColetaId, pagina, tamanho);

            var viewModels = _mapper.Map<IEnumerable<ColetaAgendadaViewModel>>(coletas);
            return Ok(viewModels);
        }
    }
}
