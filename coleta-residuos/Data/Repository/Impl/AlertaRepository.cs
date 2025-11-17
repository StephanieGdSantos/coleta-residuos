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

        public IEnumerable<AlertaModel> GetAll() => _context.Alertas
            .Include(a => a.PontoColeta)
            .ToList();

        public IEnumerable<AlertaModel> GetAll(int page, int size)
        {
            return _context.Alertas.Include(a => a.PontoColeta)
                            .Skip((page - 1) * page)
                            .Take(size)
                            .AsNoTracking()
                            .ToList();
        }

        public IEnumerable<AlertaModel> GetAllReference(int lastReference, int size)
        {
            var alertas = _context.Alertas.Include(a => a.PontoColeta)
                                .Where(a => a.Id > lastReference)
                                .OrderBy(a => a.Id)
                                .Take(size)
                                .AsNoTracking()
                                .ToList();

            return alertas;
        }

        public AlertaModel GetById(int id) => _context.Alertas.Find(id);

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
