using coleta_residuos.Data.Contexts;
using coleta_residuos.Models;
using Microsoft.EntityFrameworkCore;

namespace coleta_residuos.Data.Repository.Impl
{
    public class ResiduoRepository : IRepository<ResiduoModel>
    {
        private readonly DatabaseContext _context;

        public ResiduoRepository(DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<ResiduoModel> GetAll(int page, int size)
        {
            return _context.Residuos.Include(a => a.PontosColetaResiduos)
                            .Skip((page - 1) * page)
                            .Take(size)
                            .AsNoTracking()
                            .ToList();
        }

        public ResiduoModel GetById(int id) => _context.Residuos.Find(id);

        public void Add(ResiduoModel residuo)
        {
            _context.Residuos.Add(residuo);
            _context.SaveChanges();
        }

        public void Update(ResiduoModel residuo)
        {
            _context.Update(residuo);
            _context.SaveChanges();
        }

        public void Delete(ResiduoModel residuo)
        {
            _context.Residuos.Remove(residuo);
            _context.SaveChanges();
        }
    }
}
