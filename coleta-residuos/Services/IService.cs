using coleta_residuos.Models;

namespace coleta_residuos.Services
{
    public interface IService<TEntity>
    {
        IEnumerable<TEntity> Listar(int pagina = 0, int tamanho = 10);
        TEntity ObterPorId(int id);
        void Criar(TEntity entidade);
        void Atualizar(TEntity entidade);
        void Deletar(int id);
    }
}
