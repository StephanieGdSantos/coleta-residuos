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
        [Authorize(Roles = "operador,analista,gerente,visitante")]
        public ActionResult<IEnumerable<PontoColetaViewModel>> Get([FromQuery] int pagina = 0,
            [FromQuery] int tamanho = 10)
        {
            try
            {
                var pontos = _pontoColetaService.Listar(pagina, tamanho);

                var pontosColetaViewModel = _mapper.Map<IEnumerable<PontoColetaViewModel>>(pontos);
                return Ok(pontosColetaViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "operador,analista,gerente,visitante")]
        public ActionResult<PontoColetaViewModel> Get(int id)
        {
            try
            {
                var pontoColeta = _pontoColetaService.ObterPorId(id);
                if (pontoColeta == null)
                    return NotFound();

                var pontoColetaViewModel = _mapper.Map<PontoColetaViewModel>(pontoColeta);
                return Ok(pontoColetaViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}/Residuo")]
        [Authorize(Roles = "operador,analista,gerente,visitante")]
        public ActionResult<IEnumerable<ResiduoViewModel>> Get(int id,
            [FromQuery] int pagina = 0, [FromQuery] int tamanho = 10)
        {
            try
            {
                var pontoColeta = _pontoColetaService.ObterPorId(id);
                if (pontoColeta == null)
                    return NotFound("Ponto de coleta não encontrado.");

                var residuos = _pontoColetaResiduoService.ListarResiduosDoPontoDeColeta(id, pagina, tamanho);

                var residuosViewModel = _mapper.Map<IEnumerable<ResiduoViewModel>>(residuos);
                return Ok(residuosViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "gerente")]
        public ActionResult<PontoColetaViewModel> Post([FromBody] CriarPontoColetaViewModel criarViewModel)
        {
            var pontoColeta = _mapper.Map<PontoColetaModel>(criarViewModel);

            try
            {
                _pontoColetaService.Criar(pontoColeta);
                return CreatedAtAction(nameof(Post), new { id = pontoColeta.Id }, criarViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "gerente")]
        public ActionResult Put(int id, [FromBody] AtualizarPontoColetaViewModel atualizarViewModel)
        {
            if (id != atualizarViewModel.Id)
                return BadRequest();

            try
            {
                var pontoExistente = _pontoColetaService.ObterPorId(id);
                if (pontoExistente == null)
                    return NotFound();

                _mapper.Map(atualizarViewModel, pontoExistente);
                _pontoColetaService.Atualizar(pontoExistente);

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
                var ponto = _pontoColetaService.ObterPorId(id);
                if (ponto == null)
                    return NotFound();

                _pontoColetaService.Deletar(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
