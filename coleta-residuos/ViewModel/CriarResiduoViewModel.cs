using System.ComponentModel.DataAnnotations;

namespace coleta_residuos.ViewModel
{
    public class CriarResiduoViewModel
    {
        [Required(ErrorMessage = "O nome do resíduo é obrigatório.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "A categoria do resíduo é obrigatória.")]
        public string Categoria { get; set; }
        public string Descricao { get; set; }
    }

}
