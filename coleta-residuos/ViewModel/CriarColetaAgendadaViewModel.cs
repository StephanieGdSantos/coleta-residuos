using System.ComponentModel.DataAnnotations;

namespace coleta_residuos.ViewModel
{
    public class CriarColetaAgendadaViewModel
    {
        [Required(ErrorMessage = "A data do agendamento é obrigatória.")]
        public DateTime DataAgendada { get; set; }
        public string? Observacao { get; set; }

        [Required(ErrorMessage = "O id do ponto de coleta é obrigatório.")]
        public int PontoColetaId { get; set; }
    }
}
