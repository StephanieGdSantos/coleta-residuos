using coleta_residuos.Data.Repository;
using coleta_residuos.Models;

namespace coleta_residuos.Services.Impl
{
    public class AlertaService : IAlertaService
    {
        private readonly IRepository<AlertaModel> _repository;

        public AlertaService(IRepository<AlertaModel> repository)
        {
            _repository = repository;
        }

        public void Atualizar(AlertaModel alerta) => _repository.Update(alerta);

        public void Criar(AlertaModel alerta) => _repository.Add(alerta);

        public void Deletar(int id)
        {
            var alerta = _repository.GetById(id);
            if (alerta != null)
            {
                _repository.Delete(alerta);
            }
        }

        public IEnumerable<AlertaModel> Listar(int pagina = 0, int tamanho = 10)
        {
            return _repository
                .GetAll(pagina, tamanho);
        }

        public IEnumerable<AlertaModel> ListarPorPontoDeColeta(int pontoColetaId, int pagina = 0, int tamanho = 10)
        {
            return _repository
                .GetAll(pagina, tamanho)
                .Where(a => a.PontoColetaId == pontoColetaId);
        }

        public AlertaModel ObterPorId(int id)
        {
            return _repository
                .GetById(id);
        }
    }
}
