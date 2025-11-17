using coleta_residuos.Models;

namespace coleta_residuos.Data.Repository
{
    public interface IRepository<TEntity>
    {
        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> GetAll(int page, int size);

        IEnumerable<TEntity> GetAllReference(int lastReference, int size);

        TEntity GetById(int id);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
