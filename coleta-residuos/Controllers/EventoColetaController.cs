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
    public class EventoColetaController : ControllerBase
    {
        private readonly IEventoColetaService _eventoColetaService;
        private readonly IService<PontoColetaModel> _pontoColetaService;
        private readonly IMapper _mapper;

        public EventoColetaController(IEventoColetaService eventoColetaService, 
            IService<PontoColetaModel> pontoColetaService, IMapper mapper)
        {
            _eventoColetaService = eventoColetaService;
            _pontoColetaService = pontoColetaService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<EventoColetaViewModel>> Get([FromQuery] int pagina = 0, [FromQuery] int tamanho = 10)
        {
            try
            var eventos = _eventoColetaService.Listar(pagina, tamanho);
            var viewModels = _mapper.Map<IEnumerable<EventoColetaViewModel>>(eventos);
            return Ok(viewModels);
        }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<EventoColetaViewModel> Get(int id)
        {
            try
            {
            var evento = _eventoColetaService.ObterPorId(id);
            if (evento == null)
                return NotFound();

            var viewModel = _mapper.Map<EventoColetaViewModel>(evento);
            return Ok(viewModel);
        }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<EventoColetaViewModel> Post([FromBody] CriarEventoColetaViewModel eventoViewModel)
        {
            var evento = _mapper.Map<EventoColetaModel>(eventoViewModel);
            _eventoColetaService.Criar(evento);

            try
            {
        }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] AtualizarEventoColetaViewModel eventoViewModel)
        {
            if (id != eventoViewModel.Id)
                return BadRequest();

            try
            {
                return NotFound();

            var evento = _mapper.Map(eventoViewModel, existente);
            _eventoColetaService.Atualizar(evento);
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
            var evento = _eventoColetaService.ObterPorId(id);
            if (evento == null)
                return NotFound();

            _eventoColetaService.Deletar(id);
            return NoContent();
        }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("/api/PontoColeta/{pontoColetaId}/EventoColeta")]
        public ActionResult<IEnumerable<EventoColetaViewModel>> Get(int pontoColetaId, 
            [FromQuery] int pagina = 0, [FromQuery] int tamanho = 10)
        {
            try
            {
                return NotFound("Ponto de coleta não encontrado.");

            var eventos = _eventoColetaService.ListarPorPontoDeColeta(pontoColetaId, pagina, tamanho);

            var viewModels = _mapper.Map<IEnumerable<EventoColetaViewModel>>(eventos);
            return Ok(viewModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
