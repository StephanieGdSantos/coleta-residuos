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
        [Authorize(Roles = "operador,analista,gerente")]
        public ActionResult<IEnumerable<ColetaAgendadaViewModel>> Get([FromQuery] int pagina = 0, [FromQuery] int tamanho = 10)
        {
            try
            {
                var coletas = _coletaAgendadaService.Listar(pagina, tamanho);

                var coletasViewModel = _mapper.Map<IEnumerable<ColetaAgendadaViewModel>>(coletas);
                return Ok(coletasViewModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "operador,analista,gerente")]
        public ActionResult<ColetaAgendadaViewModel> Get(int id)
        {
            try
            {
                var coleta = _coletaAgendadaService.ObterPorId(id);
                if (coleta == null)
                    return NotFound("Agendamento não encontrado.");

                var coletaViewModel = _mapper.Map<ColetaAgendadaViewModel>(coleta);
                return Ok(coletaViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "operador,gerente")]
        public ActionResult<ColetaAgendadaViewModel> Post([FromBody] CriarColetaAgendadaViewModel criarColetaViewModel)
        {
            var coleta = _mapper.Map<ColetaAgendadaModel>(criarColetaViewModel);

            try
            {
                _coletaAgendadaService.Criar(coleta);

                var coletaCriada = _mapper.Map<ColetaAgendadaViewModel>(coleta);
                return CreatedAtAction(nameof(Post), coletaCriada);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "operador,gerente")]
        public ActionResult Put(int id, [FromBody] AtualizarColetaAgendadaViewModel atualizarColetaViewModel)
        {
            if (id != atualizarColetaViewModel.Id)
                return BadRequest("O id fornecido é divergente do agendamento que será atualizado.");

            try
            {
                var coletaExistente = _coletaAgendadaService.ObterPorId(id);
                if (coletaExistente == null)
                    return NotFound("Agendamento não encontrado.");

                _mapper.Map(atualizarColetaViewModel, coletaExistente);
                _coletaAgendadaService.Atualizar(coletaExistente);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "operador,gerente")]
        public ActionResult Delete(int id)
        {
            try
            {
                var coleta = _coletaAgendadaService.ObterPorId(id);
                if (coleta == null)
                    return NotFound("Agendamento não encontrado.");

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
        [Authorize(Roles = "operador,analista,gerente")]
        public ActionResult<IEnumerable<ColetaAgendadaViewModel>> Get(int pontoColetaId, [FromQuery] int pagina = 0, [FromQuery] int tamanho = 10)
        {
            try
            {
                var pontoColeta = _pontoColetaService.ObterPorId(pontoColetaId);
                if (pontoColeta == null)
                    return NotFound("Ponto de coleta não encontrado.");

                var coletas = _coletaAgendadaService.ListarPorPontoDeColeta(pontoColetaId, pagina, tamanho);

                var coletasViewModel = _mapper.Map<IEnumerable<ColetaAgendadaViewModel>>(coletas);
                return Ok(coletasViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
