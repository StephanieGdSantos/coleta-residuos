using System.ComponentModel.DataAnnotations;

namespace coleta_residuos.ViewModel
{
    public class AtualizarResiduoViewModel
    {
        [Required(ErrorMessage = "É obrigatório informar o id do resíduo que será editado.")]
        public string Id { get; set; }

        [Required(ErrorMessage = "O nome do resíduo é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A categoria do resíduo é obrigatória.")]
        public string Categoria { get; set; }
        public string Descricao { get; set; }
    }
}
