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
    public class ResiduoController : ControllerBase
    {
        private readonly IService<ResiduoModel> _residuoService;
        private readonly IPontoColetaResiduoService _pontoColetaResiduoService;
        private readonly IMapper _mapper;

        public ResiduoController(IService<ResiduoModel> residuoService, 
            IPontoColetaResiduoService pontoColetaResiduoService, IMapper mapper)
        {
            _residuoService = residuoService;
            _pontoColetaResiduoService = pontoColetaResiduoService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ResiduoViewModel>> Get([FromQuery] int pagina = 0, [FromQuery] int tamanho = 10)
        {
            var residuos = _residuoService.Listar(pagina, tamanho);
            var viewModels = _mapper.Map<IEnumerable<ResiduoViewModel>>(residuos);
            return Ok(viewModels);
        }

        [HttpGet("{id}")]
        public ActionResult<ResiduoViewModel> Get(int id)
        {
            var residuo = _residuoService.ObterPorId(id);
            if (residuo == null)
                return NotFound();

            var viewModel = _mapper.Map<ResiduoViewModel>(residuo);
            return Ok(viewModel);
        }

        [HttpGet("{id}/pontos-coleta")]
        public ActionResult<IEnumerable<PontoColetaViewModel>> Get(int id, 
            [FromQuery] int pagina = 0, [FromQuery] int tamanho = 10)
        {
            var pontos = _pontoColetaResiduoService.ListarPontosDeColetaPorResiduo(id, pagina, tamanho);

            var viewModels = _mapper.Map<IEnumerable<PontoColetaViewModel>>(pontos);
            return Ok(viewModels);
        }

        [HttpPost]
        public ActionResult<ResiduoViewModel> Post([FromBody] CriarResiduoViewModel criarViewModel)
        {
            var residuo = _mapper.Map<ResiduoModel>(criarViewModel);
            _residuoService.Criar(residuo);

            var viewModel = _mapper.Map<ResiduoViewModel>(residuo);
            return CreatedAtAction(nameof(Post), new { id = residuo.Id }, viewModel);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] AtualizarResiduoViewModel atualizarViewModel)
        {
            if (id.ToString() != atualizarViewModel.Id)
                return BadRequest();

            var existente = _residuoService.ObterPorId(id);
            if (existente == null)
                return NotFound();

            var residuo = _mapper.Map(atualizarViewModel, existente);
            _residuoService.Atualizar(residuo);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var residuo = _residuoService.ObterPorId(id);
            if (residuo == null)
                return NotFound();

            _residuoService.Deletar(id);
            return NoContent();
        }
    }
}
