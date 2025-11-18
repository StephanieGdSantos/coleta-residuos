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
            var pontos = _pontoColetaService.Listar(pagina, tamanho);
            var viewModels = _mapper.Map<IEnumerable<PontoColetaViewModel>>(pontos);
            return Ok(viewModels);
        }

        [HttpGet("{id}")]
        public ActionResult<PontoColetaViewModel> Get(int id)
        {
            var ponto = _pontoColetaService.ObterPorId(id);
            if (ponto == null)
                return NotFound();

            var viewModel = _mapper.Map<PontoColetaViewModel>(ponto);
            return Ok(viewModel);
        }

        [HttpGet("{id}/Residuo")]
        public ActionResult<IEnumerable<ResiduoViewModel>> Get(int id,
            [FromQuery] int pagina = 0, [FromQuery] int tamanho = 10)
        {
            var residuos = _pontoColetaResiduoService.ListarResiduosDoPontoDeColeta(id, pagina, tamanho);

            var viewModels = _mapper.Map<IEnumerable<ResiduoViewModel>>(residuos);
            return Ok(viewModels);
        }

        [HttpPost]
        public ActionResult<PontoColetaViewModel> Post([FromBody] CriarPontoColetaViewModel criarViewModel)
        {
            var ponto = _mapper.Map<PontoColetaModel>(criarViewModel);
            _pontoColetaService.Criar(ponto);

            var result = _mapper.Map<PontoColetaViewModel>(ponto);
            return CreatedAtAction(nameof(Post), new { id = ponto.Id }, result);
        }


        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] AtualizarPontoColetaViewModel atualizarViewModel)
        {
            var pontoExistente = _pontoColetaService.ObterPorId(id);
            if (pontoExistente == null)
                return NotFound();

            _mapper.Map(atualizarViewModel, pontoExistente);
            _pontoColetaService.Atualizar(pontoExistente);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var ponto = _pontoColetaService.ObterPorId(id);
            if (ponto == null)
                return NotFound();

            _pontoColetaService.Deletar(id);
            return NoContent();
        }
    }
}
