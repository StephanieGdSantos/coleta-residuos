using coleta_residuos.Models;

namespace coleta_residuos.ViewModel
{
    public class EventoColetaViewModel
    {
        public int Id { get; set; }
        public int PontoColetaId { get; set; }
        public DateTime DataEvento { get; set; }
        public double QuantidadeKg { get; set; }
        public TipoEvento TipoEvento { get; set; }
    }
}
