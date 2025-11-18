using coleta_residuos.Data.Contexts;
using coleta_residuos.Models;

namespace coleta_residuos.Data.Repository.Impl
{
    public class PontoColetaResiduoRepository : IPontoColetaResiduoRepository
    {
        private readonly DatabaseContext _context;

        public PontoColetaResiduoRepository(DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<ResiduoModel> GetResiduosPorPontoColetaId(int pontoColetaId, 
            int pagina = 0, int tamanho = 10)
        {
            return _context.PontoColetaResiduos
                .Where(pcr => pcr.PontoColetaId == pontoColetaId)
                .Select(pcr => pcr.Residuo)
                .Skip(pagina * tamanho)
                .Take(tamanho)
                .ToList();
        }

        public IEnumerable<PontoColetaModel> GetPontosColetaPorResiduoId(int residuoId, 
            int pagina = 0, int tamanho = 10)
        {
            return _context.PontoColetaResiduos
                .Where(pcr => pcr.ResiduoId == residuoId)
                .Select(pcr => pcr.PontoColeta)
                .Skip(pagina * tamanho)
                .Take(tamanho)
                .ToList();
        }
    }
}
