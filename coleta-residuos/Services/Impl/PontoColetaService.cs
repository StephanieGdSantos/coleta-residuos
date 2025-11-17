using coleta_residuos.Data.Repository;
using coleta_residuos.Models;

namespace coleta_residuos.Services.Impl
{
    public class PontoColetaService : IService<PontoColetaModel>
    {
        private readonly IRepository<PontoColetaModel> _repository;

        public PontoColetaService(IRepository<PontoColetaModel> repository)
        {
            _repository = repository;
        }

        public void Atualizar(PontoColetaModel pontoColeta) => _repository.Update(pontoColeta);

        public void Criar(PontoColetaModel pontoColeta) => _repository.Add(pontoColeta);

        public void Deletar(int id)
        {
            var pontoColeta = _repository.GetById(id);
            if (pontoColeta != null)
            {
                _repository.Delete(pontoColeta);
            }
        }

        public IEnumerable<PontoColetaModel> Listar(int pagina = 0, int tamanho = 10)
        {
            return _repository
                .GetAll(pagina, tamanho);
        }

        public PontoColetaModel ObterPorId(int id)
        {
            return _repository
                .GetById(id);
        }
    }
}
