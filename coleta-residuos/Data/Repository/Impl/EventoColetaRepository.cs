using coleta_residuos.Data.Contexts;
using coleta_residuos.Models;
using Microsoft.EntityFrameworkCore;

namespace coleta_residuos.Data.Repository.Impl
{
    public class EventoColetaRepository : IRepository<EventoColetaModel>
    {
        private readonly DatabaseContext _context;

        public EventoColetaRepository(DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<EventoColetaModel> GetAll(int page, int size)
        {
            return _context.EventosColeta
                            .Skip((page - 1) * page)
                            .Take(size)
                            .AsNoTracking()
                            .ToList()
                            .OrderBy(e => e.Id);
        }

        public EventoColetaModel GetById(int id) => _context.EventosColeta.Find(id);

        public void Add(EventoColetaModel eventoColeta)
        {
            _context.EventosColeta.Add(eventoColeta);
            _context.SaveChanges();
        }

        public void Update(EventoColetaModel eventoColeta)
        {
            _context.Update(eventoColeta);
            _context.SaveChanges();
        }

        public void Delete(EventoColetaModel eventoColeta)
        {
            _context.EventosColeta.Remove(eventoColeta);
            _context.SaveChanges();
        }
    }
}
