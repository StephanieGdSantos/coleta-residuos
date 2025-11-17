using System.Drawing;
using static coleta_residuos.Models.PontoColetaModel;

namespace coleta_residuos.Models
{
    public class ColetaAgendadaModel
    {
        public int Id { get; set; }
        public DateTime DataAgendada { get; set; }
        public string? Observacao { get; set; }
        public int PontoColetaId { get; set; }
        public PontoColetaModel PontoColeta { get; set; } = null!;
    }

}
