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
    [Authorize]
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
        [Authorize(Roles = "operador,analista,gerente")]
        public ActionResult<IEnumerable<EventoColetaViewModel>> Get([FromQuery] int pagina = 0, [FromQuery] int tamanho = 10)
        {
            try
            {
                var eventos = _eventoColetaService.Listar(pagina, tamanho);

                var eventosViewModel = _mapper.Map<IEnumerable<EventoColetaViewModel>>(eventos);
                return Ok(eventosViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "operador,analista,gerente")]
        public ActionResult<EventoColetaViewModel> Get(int id)
        {
            try
            {
                var evento = _eventoColetaService.ObterPorId(id);
                if (evento == null)
                    return NotFound();

                var eventoViewModel = _mapper.Map<EventoColetaViewModel>(evento);
                return Ok(eventoViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "operador,gerente")]
        public ActionResult<EventoColetaViewModel> Post([FromBody] CriarEventoColetaViewModel criarEventoViewModel)
        {
            var evento = _mapper.Map<EventoColetaModel>(criarEventoViewModel);

            try
            {
                _eventoColetaService.Criar(evento);
                return CreatedAtAction(nameof(Post), new { id = evento.Id }, criarEventoViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "operador,gerente")]
        public ActionResult Put(int id, [FromBody] AtualizarEventoColetaViewModel atualizarEventoViewModel)
        {
            if (id != atualizarEventoViewModel.Id)
                return BadRequest();

            try
            {
                var eventoExistente = _eventoColetaService.ObterPorId(id);
                if (eventoExistente == null)
                    return NotFound();

                _mapper.Map(atualizarEventoViewModel, eventoExistente);
                _eventoColetaService.Atualizar(eventoExistente);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "gerente")]
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
                var pontoColetaExistente = _pontoColetaService.ObterPorId(pontoColetaId);
                if (pontoColetaExistente == null)
                    return NotFound("Ponto de coleta não encontrado.");

                var eventos = _eventoColetaService.ListarPorPontoDeColeta(pontoColetaId, pagina, tamanho);

                var eventosViewModel = _mapper.Map<IEnumerable<EventoColetaViewModel>>(eventos);
                return Ok(eventosViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
