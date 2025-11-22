using coleta_residuos.Data.Contexts;
using coleta_residuos.Models;
using Microsoft.EntityFrameworkCore;

namespace coleta_residuos.Data.Repository.Impl
{
    public class AlertaRepository : IRepository<AlertaModel>
    {
        private readonly DatabaseContext _context;

        public AlertaRepository(DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<AlertaModel> GetAll(int page, int size)
        {
            return _context.Alertas.Include(a => a.PontoColeta)
                                .ThenInclude(a => a.PontosColetaResiduos)
                                    .ThenInclude(a => a.Residuo)
                            .Skip((page - 1) * page)
                            .Take(size)
                            .AsNoTracking()
                            .ToList()
                            .OrderBy(a => a.Id);
        }

        public AlertaModel GetById(int id)
        {
            return _context.Alertas
                .Include(c => c.PontoColeta)
                    .ThenInclude(c => c.PontosColetaResiduos)
                        .ThenInclude(c => c.Residuo)
                .AsNoTracking()
                .FirstOrDefault(c => c.Id == id);
        }

        public void Add(AlertaModel alerta)
        {
            _context.Alertas.Add(alerta);
            _context.SaveChanges();
        }

        public void Update(AlertaModel alerta)
        {
            _context.Update(alerta);
            _context.SaveChanges();
        }

        public void Delete(AlertaModel alerta)
        {
            _context.Alertas.Remove(alerta);
            _context.SaveChanges();
        }
    }
}
