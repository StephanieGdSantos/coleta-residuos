using coleta_residuos.Data.Contexts;
using coleta_residuos.Models;
using Microsoft.EntityFrameworkCore;

namespace coleta_residuos.Data.Repository.Impl
{
    public class ColetaAgendadaRepository : IRepository<ColetaAgendadaModel>
    {
        private readonly DatabaseContext _context;

        public ColetaAgendadaRepository(DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<ColetaAgendadaModel> GetAll(int page, int size)
        {
            return _context.ColetasAgendadas.Include(c => c.PontoColeta)
                                .ThenInclude(c => c.PontosColetaResiduos)
                                    .ThenInclude(c => c.Residuo)
                            .Skip((page - 1) * page)
                            .Take(size)
                            .AsNoTracking()
                            .ToList();
        }

        public ColetaAgendadaModel GetById(int id)
        {
            return _context.ColetasAgendadas
                .Include(c => c.PontoColeta)
                    .ThenInclude(c => c.PontosColetaResiduos)
                        .ThenInclude(c => c.Residuo)
                .AsNoTracking()
                .FirstOrDefault(c => c.Id == id); 
        }

        public void Add(ColetaAgendadaModel coletaAgendada)
        {
            _context.ColetasAgendadas.Add(coletaAgendada);
            _context.SaveChanges();
        }

        public void Update(ColetaAgendadaModel coletaAgendada)
        {
            _context.Update(coletaAgendada);
            _context.SaveChanges();
        }

        public void Delete(ColetaAgendadaModel coletaAgendada)
        {
            _context.ColetasAgendadas.Remove(coletaAgendada);
            _context.SaveChanges();
        }
    }
}
