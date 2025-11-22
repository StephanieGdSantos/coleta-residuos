using coleta_residuos.Data.Contexts;
using coleta_residuos.Models;
using Microsoft.EntityFrameworkCore;

namespace coleta_residuos.Data.Repository.Impl
{
    public class PontoColetaRepository : IRepository<PontoColetaModel>
    {
        private readonly DatabaseContext _context;

        public PontoColetaRepository(DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<PontoColetaModel> GetAll(int page, int size)
        {
            return _context.PontosColeta.Include(p => p.PontosColetaResiduos)
                            .Skip((page - 1) * page)
                            .Take(size)
                            .AsNoTracking()
                            .ToList();
        }

        public PontoColetaModel GetById(int id) => _context.PontosColeta.Find(id);

        public void Add(PontoColetaModel pontoColeta)
        {
            _context.PontosColeta.Add(pontoColeta);
            _context.SaveChanges();
        }

        public void Update(PontoColetaModel pontoColeta)
        {
            _context.Update(pontoColeta);
            _context.SaveChanges();
        }

        public void Delete(PontoColetaModel pontoColeta)
        {
            _context.PontosColeta.Remove(pontoColeta);
            _context.SaveChanges();
        }
    }
}
