using coleta_residuos.Data.Repository;
using coleta_residuos.Models;

namespace coleta_residuos.Services.Impl
{
    public class ColetaAgendadaService : IColetaAgendadaService
    {
        private readonly IRepository<ColetaAgendadaModel> _repository;

        public ColetaAgendadaService(IRepository<ColetaAgendadaModel> repository)
        {
            _repository = repository;
        }

        public void Atualizar(ColetaAgendadaModel coletaAgendada) => _repository.Update(coletaAgendada);

        public void Criar(ColetaAgendadaModel coletaAgendada) => _repository.Add(coletaAgendada);

        public void Deletar(int id)
        {
            var coletaAgendada = _repository.GetById(id);
            if (coletaAgendada != null)
            {
                _repository.Delete(coletaAgendada);
            }
        }

        public IEnumerable<ColetaAgendadaModel> Listar(int pagina = 0, int tamanho = 10)
        {
            return _repository
                .GetAll(pagina, tamanho);
        }

        public IEnumerable<ColetaAgendadaModel> ListarPorPontoDeColeta(int pontoColetaId, 
            int pagina = 0, int tamanho = 10)
        {
            return _repository
                .GetAll(pagina, tamanho)
                .Where(ca => ca.PontoColetaId == pontoColetaId);
        }

        public ColetaAgendadaModel ObterPorId(int id)
        {
            return _repository
                .GetById(id);
        }
    }
}
