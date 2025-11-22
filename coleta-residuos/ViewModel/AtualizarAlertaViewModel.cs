using System.ComponentModel.DataAnnotations;

namespace coleta_residuos.ViewModel
{
    public class AtualizarAlertaViewModel
    {
        [Required(ErrorMessage = "É obrigatório informar o id do alerta que será editado.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "É obrigatório informar o id do ponto de coleta relacionado ao alerta.")]
        public int PontoColetaId { get; set; }

        [Required(ErrorMessage = "É obrigatório informar a mensagem do alerta.")]
        public string Mensagem { get; set; }
        public bool Resolvido { get; set; }
    }
}
