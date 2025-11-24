using System.ComponentModel.DataAnnotations;

namespace coleta_residuos.ViewModel
{
    public class CriarColetaAgendadaViewModel
    {
        [Required(ErrorMessage = "A data do agendamento é obrigatória.")]
        [Range(typeof(DateTime), "01/01/2000", "12/31/2100", ErrorMessage = "A data do agendamento deve " +
            "estar entre 01/01/2000 e 31/12/2100.")]
        public DateTime DataAgendada { get; set; }
        public string? Observacao { get; set; }

        [Required(ErrorMessage = "O id do ponto de coleta é obrigatório.")]
        public int PontoColetaId { get; set; }
    }
}
