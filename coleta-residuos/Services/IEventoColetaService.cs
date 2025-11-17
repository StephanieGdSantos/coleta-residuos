using coleta_residuos.Models;

namespace coleta_residuos.Services
{
    public interface IEventoColetaService : IService<EventoColetaModel>
    {
        IEnumerable<EventoColetaModel> ListarPorPontoDeColeta(int pontoColetaId, int pagina = 0, int tamanho = 10);
    }
}
