using System.ComponentModel.DataAnnotations;

namespace coleta_residuos.ViewModel
{
    public class CriarAlertaViewModel
    {
        [Required(ErrorMessage = "O id do ponto de coleta é obrigatório.")]
        public int PontoColetaId { get; set; }
        [Required(ErrorMessage = "O alerta deve ter uma mensagem.")]
        public string Mensagem { get; set; }
    }

}
