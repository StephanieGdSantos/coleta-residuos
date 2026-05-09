using System.ComponentModel.DataAnnotations;

namespace coleta_residuos.ViewModel
{
    public class AtualizarColetaAgendadaViewModel
    {
        [Required(ErrorMessage = "É obrigatório informar o id da coleta agendada que será editada.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "É obrigatório informar a data agendada para a coleta.")]
        public DateTime DataAgendada { get; set; }

        public string? Observacao { get; set; }

        [Required(ErrorMessage = "O id do ponto de coleta é obrigatório.")]
        public int PontoColetaId { get; set; }
    }
}
