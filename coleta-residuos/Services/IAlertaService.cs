using coleta_residuos.Models;

namespace coleta_residuos.Services
{
    public interface IAlertaService : IService<AlertaModel>
    {
        IEnumerable<AlertaModel> ListarPorPontoDeColeta(int pontoColetaId, int pagina = 0, int tamanho = 10);
    }
}
