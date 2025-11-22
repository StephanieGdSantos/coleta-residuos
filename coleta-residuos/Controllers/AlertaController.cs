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
            var alertas = _alertaService.Listar(pagina, tamanho);

            var viewModels = _mapper.Map<IEnumerable<AlertaViewModel>>(alertas);
            return Ok(viewModels);
        }

        [HttpGet("{id}")]
        public ActionResult<AlertaViewModel> Get(int id)
        {
            var alerta = _alertaService.ObterPorId(id);
            if (alerta == null)
                return NotFound();

            var viewModel = _mapper.Map<AlertaViewModel>(alerta);
            return Ok(viewModel);
        }

        [HttpPost]
        public ActionResult<AlertaViewModel> Post([FromBody] CriarAlertaViewModel alertaViewModel)
        {
            var alerta = _mapper.Map<AlertaModel>(alertaViewModel);
            alerta.DataAlerta = DateTime.UtcNow.AddHours(-3);

            var pontoColeta = _pontoColetaService.ObterPorId(alerta.PontoColetaId);

            if (pontoColeta == null)
                return BadRequest("Ponto de Coleta não encontrado.");

            _alertaService.Criar(alerta);

            var viewModel = _mapper.Map<AlertaViewModel>(alerta);
            return CreatedAtAction(nameof(Post), new { id = alerta.Id }, viewModel);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] AtualizarAlertaViewModel alertaViewModel)
        {
            if (id != alertaViewModel.Id)
                return BadRequest();

            var existente = _alertaService.ObterPorId(id);
            if (existente == null)
                return NotFound();

            var alerta = _mapper.Map<AlertaModel>(alertaViewModel);
            _alertaService.Atualizar(alerta);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var alerta = _alertaService.ObterPorId(id);
            if (alerta == null)
                return NotFound();

            _alertaService.Deletar(id);
            return NoContent();
        }

        [HttpGet]
        [Route("/api/PontoColeta/{pontoColetaId}/Alerta")]
        public ActionResult<IEnumerable<AlertaViewModel>> Get(int pontoColetaId, [FromQuery] int pagina = 0, [FromQuery] int tamanho = 10)
        {
            var pontoColeta = _pontoColetaService.ObterPorId(pontoColetaId);
            if (pontoColeta == null)
                return NotFound("Ponto de coleta não encontrado.");

            var alertas = _alertaService.ListarPorPontoDeColeta(pontoColetaId, pagina, tamanho);

            var viewModels = _mapper.Map<IEnumerable<AlertaViewModel>>(alertas);
            return Ok(viewModels);
        }
    }
}