using coleta_residuos.Data.Repository;
using coleta_residuos.Models;

namespace coleta_residuos.Services.Impl
{
    public class ResiduoService : IService<ResiduoModel>
    {
        private readonly IRepository<ResiduoModel> _repository;

        public ResiduoService(IRepository<ResiduoModel> repository)
        {
            _repository = repository;
        }

        public void Atualizar(ResiduoModel entidade) => _repository.Update(entidade);

        public void Criar(ResiduoModel entidade) => _repository.Add(entidade);

        public void Deletar(int id)
        {
            var entidade = _repository.GetById(id);
            if (entidade != null)
            {
                _repository.Delete(entidade);
            }
        }

        public IEnumerable<ResiduoModel> Listar(int pagina = 0, int tamanho = 10)
        {
            return _repository
                .GetAll(pagina, tamanho);
        }

        public ResiduoModel ObterPorId(int id)
        {
            return _repository
                .GetById(id);
        }
    }
}
