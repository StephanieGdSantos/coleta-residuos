using System.Drawing;

namespace coleta_residuos.Models
{
    public class EventoColetaModel
    {
        public int Id { get; set; }
        public int PontoColetaId { get; set; }
        public PontoColetaModel PontoColeta { get; set; }

        public DateTime DataEvento { get; set; }
        public double QuantidadeKg { get; set; }

        public TipoEvento TipoEvento { get; set; }
    }

    public enum TipoEvento
    {
        Deposito = 1,
        Coleta = 2
    }

}
