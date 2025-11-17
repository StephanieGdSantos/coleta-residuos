using coleta_residuos.Models;

namespace coleta_residuos.Services
{
    public interface IPontoColetaResiduoService
    {
        IEnumerable<PontoColetaModel> ListarPontosDeColetaPorResiduo(int residuoId, int pagina = 0, int tamanho = 10);
        IEnumerable<ResiduoModel> ListarResiduosDoPontoDeColeta(int pontoColetaId, int pagina = 0, int tamanho = 10);
    }
}
