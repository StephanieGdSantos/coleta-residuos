using AutoMapper;
using coleta_residuos.Models;
using coleta_residuos.Services;
using coleta_residuos.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace coleta_residuos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class AlertaController : ControllerBase
    {
        private readonly IAlertaService _alertaService;
        private readonly IService<PontoColetaModel> _pontoColetaService;
        private readonly IMapper _mapper;

        public AlertaController(IAlertaService alertaService, IService<PontoColetaModel> pontoColetaService, 
            IMapper mapper)
        {
            _alertaService = alertaService;
            _pontoColetaService = pontoColetaService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<AlertaViewModel>> Get([FromQuery] int pagina = 0, 
            [FromQuery] int tamanho = 10)
        {
            try
            {
                var alertas = _alertaService.Listar(pagina, tamanho);

                var alertasViewModel = _mapper.Map<IEnumerable<AlertaViewModel>>(alertas);
                return Ok(alertasViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<AlertaViewModel> Get(int id)
        {
            try
            {
                var alerta = _alertaService.ObterPorId(id);
                if (alerta == null)
                    return NotFound();

                var alertaViewModel = _mapper.Map<AlertaViewModel>(alerta);
                return Ok(alertaViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<AlertaViewModel> Post([FromBody] CriarAlertaViewModel criarAlertaViewModel)
        {
            var alerta = _mapper.Map<AlertaModel>(criarAlertaViewModel);
            alerta.DataAlerta = DateTime.UtcNow.AddHours(-3);

            try
            {
                var pontoColetaExistente = _pontoColetaService.ObterPorId(alerta.PontoColetaId);

                if (pontoColetaExistente == null)
                    return BadRequest("Ponto de Coleta não encontrado.");

                _alertaService.Criar(alerta);
                return CreatedAtAction(nameof(Post), new { id = alerta.Id }, criarAlertaViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] AtualizarAlertaViewModel atualizarAlertaViewModel)
        {
            if (id != atualizarAlertaViewModel.Id)
                return BadRequest();

            try
            {
                var alertaExistente = _alertaService.ObterPorId(id);
                if (alertaExistente == null)
                    return NotFound();

                _mapper.Map(atualizarAlertaViewModel, alertaExistente);
                _alertaService.Atualizar(alertaExistente);
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
                var alerta = _alertaService.ObterPorId(id);
                if (alerta == null)
                    return NotFound();

                _alertaService.Deletar(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("/api/PontoColeta/{pontoColetaId}/Alerta")]
        public ActionResult<IEnumerable<AlertaViewModel>> Get(int pontoColetaId, [FromQuery] int pagina = 0, 
            [FromQuery] int tamanho = 10)
        {
            try
            {
                var pontoColeta = _pontoColetaService.ObterPorId(pontoColetaId);
                if (pontoColeta == null)
                    return NotFound("Ponto de coleta não encontrado.");

                var alertas = _alertaService.ListarPorPontoDeColeta(pontoColetaId, pagina, tamanho);

                var alertasViewModel = _mapper.Map<IEnumerable<AlertaViewModel>>(alertas);
                return Ok(alertasViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}