using coleta_residuos.Models;

namespace coleta_residuos.Data.Repository
{
    public interface IPontoColetaResiduoRepository
    {
        IEnumerable<ResiduoModel> GetResiduosPorPontoColetaId(int pontoColetaId, int pagina = 0, int tamanho = 10);
        IEnumerable<PontoColetaModel> GetPontosColetaPorResiduoId(int residuoId, int pagina = 0, int tamanho = 10);
    }
}
