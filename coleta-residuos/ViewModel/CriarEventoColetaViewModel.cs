using coleta_residuos.Models;
using System.ComponentModel.DataAnnotations;

namespace coleta_residuos.ViewModel
{
    public class CriarEventoColetaViewModel
    {
        [Required(ErrorMessage = "O id do ponto de coleta é obrigatório.")]
        public int PontoColetaId { get; set; }

        [Required(ErrorMessage = "O id do resíduo é obrigatório.")]
        public int ResiduoId { get; set; }

        [Required(ErrorMessage = "A data do evento é obrigatória.")]
        [Range(typeof(DateTime), "01/01/2000", "12/31/2100", ErrorMessage = "A data do evento deve " +
            "estar entre 01/01/2000 e 31/12/2100.")]
        public DateTime DataEvento { get; set; }

        [Required(ErrorMessage = "A quantidade em kg é obrigatória.")]
        [Range(0.1, 10000, ErrorMessage = "A quantidade em kg deve ser entre 0.1 e 10000.0.")]
        public double QuantidadeKg { get; set; }

        [Required(ErrorMessage = "O tipo de evento é obrigatório.")]
        public TipoEvento TipoEvento { get; set; }
    }
}
