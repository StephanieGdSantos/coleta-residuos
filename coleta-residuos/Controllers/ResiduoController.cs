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
            try
            {
            var residuos = _residuoService.Listar(pagina, tamanho);
            var viewModels = _mapper.Map<IEnumerable<ResiduoViewModel>>(residuos);
            return Ok(viewModels);
        }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<ResiduoViewModel> Get(int id)
        {
            try
            {
            var residuo = _residuoService.ObterPorId(id);
            if (residuo == null)
                return NotFound();

            var viewModel = _mapper.Map<ResiduoViewModel>(residuo);
            return Ok(viewModel);
        }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}/PontoColeta")]
        public ActionResult<IEnumerable<PontoColetaViewModel>> Get(int id, 
            [FromQuery] int pagina = 0, [FromQuery] int tamanho = 10)
        {
            try
            {
            var residuo = _residuoService.ObterPorId(id);
            if (residuo == null)
                return NotFound("Resíduo não encontrado.");

            var pontos = _pontoColetaResiduoService.ListarPontosDeColetaPorResiduo(id, pagina, tamanho);

            var pontosColetaViewModels = _mapper.Map<IEnumerable<PontoColetaViewModel>>(pontos);
            return Ok(pontosColetaViewModels);
        }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<ResiduoViewModel> Post([FromBody] CriarResiduoViewModel criarViewModel)
        {
            var residuo = _mapper.Map<ResiduoModel>(criarViewModel);
            _residuoService.Criar(residuo);

            try
            {
        }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] AtualizarResiduoViewModel atualizarViewModel)
        {
            if (id.ToString() != atualizarViewModel.Id)
                return BadRequest();

            try
            {
                return NotFound();

            var residuo = _mapper.Map(atualizarViewModel, existente);
            _residuoService.Atualizar(residuo);
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
            var residuo = _residuoService.ObterPorId(id);
            if (residuo == null)
                return NotFound();

            _residuoService.Deletar(id);
            return NoContent();
        }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
