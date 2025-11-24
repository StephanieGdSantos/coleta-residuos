using System.ComponentModel.DataAnnotations;

namespace coleta_residuos.ViewModel
{
    public class AtualizarColetaAgendadaViewModel
    {
        [Required(ErrorMessage = "É obrigatório informar o id da coleta agendada que será editada.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "É obrigatório informar a data agendada para a coleta.")]
        [Range(typeof(DateTime), "01/01/2000", "12/31/2100", ErrorMessage = "A data do agendamento deve " +
            "estar entre 01/01/2000 e 31/12/2100.")]
        public DateTime DataAgendada { get; set; }

        public string? Observacao { get; set; }

        [Required(ErrorMessage = "O id do ponto de coleta é obrigatório.")]
        public int PontoColetaId { get; set; }
    }
}
