using coleta_residuos.Data.Repository;
using coleta_residuos.Models;

namespace coleta_residuos.Services.Impl
{
    public class PontoColetaResiduoService : IPontoColetaResiduoService
    {
        private readonly IPontoColetaResiduoRepository _repository;

        public PontoColetaResiduoService(IPontoColetaResiduoRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<PontoColetaModel> ListarPontosDeColetaPorResiduo(int residuoId, 
            int pagina = 0, int tamanho = 10)
        {
            return _repository
                .GetPontosColetaPorResiduoId(residuoId, pagina, tamanho);
        }

        public IEnumerable<ResiduoModel> ListarResiduosDoPontoDeColeta(int pontoColetaId, 
            int pagina = 0, int tamanho = 10)
        {
            return _repository
                .GetResiduosPorPontoColetaId(pontoColetaId, pagina, tamanho);
        }
    }
}
