using coleta_residuos.Models;
using System.ComponentModel.DataAnnotations;

namespace coleta_residuos.ViewModel
{
    public class AtualizarEventoColetaViewModel
    {
        [Required(ErrorMessage = "É obrigatório informar o id do evento que será editado.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O id do ponto de coleta é obrigatório.")]
        public int PontoColetaId { get; set; }

        [Required(ErrorMessage = "O id do resíduo é obrigatório.")]
        public int ResiduoId { get; set; }

        [Required(ErrorMessage = "A data do evento é obrigatória.")]
        [Range(typeof(DateTime), "1/1/2000", "12/31/2100", ErrorMessage = "A data do evento deve " +
            "estar entre 01/01/2000 e 31/12/2100.")]
        public DateTime DataEvento { get; set; }

        [Required(ErrorMessage = "A quantidade em kg é obrigatória.")]
        public double QuantidadeKg { get; set; }

        [Required(ErrorMessage = "O tipo de evento é obrigatório.")]
        [Range(1, 2, ErrorMessage = "O tipo de evento deve ser 1 (Depósito) ou 2 (Coleta).")]
        public TipoEvento TipoEvento { get; set; }
    }
}
