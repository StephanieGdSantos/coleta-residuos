using coleta_residuos.Data.Repository;
using coleta_residuos.Models;

namespace coleta_residuos.Services.Impl
{
    public class EventoColetaService : IEventoColetaService
    {
        private readonly IRepository<EventoColetaModel> _repository;

        public EventoColetaService(IRepository<EventoColetaModel> repository)
        {
            _repository = repository;
        }

        public void Atualizar(EventoColetaModel eventoColeta) => _repository.Update(eventoColeta);

        public void Criar(EventoColetaModel eventoColeta) => _repository.Add(eventoColeta);

        public void Deletar(int id)
        {
            var eventoColeta = _repository.GetById(id);
            if (eventoColeta != null)
            {
                _repository.Delete(eventoColeta);
            }
        }

        public IEnumerable<EventoColetaModel> Listar(int pagina = 0, int tamanho = 10)
        {
            return _repository
                .GetAll(pagina, tamanho);
        }

        public IEnumerable<EventoColetaModel> ListarPorPontoDeColeta(int pontoColetaId, 
            int pagina = 0, int tamanho = 10)
        {
            return _repository
                .GetAll(pagina, tamanho)
                .Where(ec => ec.PontoColetaId == pontoColetaId);
        }

        public EventoColetaModel ObterPorId(int id)
        {
            return _repository
                .GetById(id);
        }
    }
}
