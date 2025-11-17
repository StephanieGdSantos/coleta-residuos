using coleta_residuos.Models;

namespace coleta_residuos.Services
{
    public interface IColetaAgendadaService : IService<ColetaAgendadaModel>
    {
        IEnumerable<ColetaAgendadaModel> ListarPorPontoDeColeta(int pontoColetaId, int pagina = 0, int tamanho = 10);
    }
}
