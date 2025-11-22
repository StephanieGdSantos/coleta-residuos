using coleta_residuos.Models;
using System.ComponentModel.DataAnnotations;

namespace coleta_residuos.ViewModel
{
    public class AtualizarPontoColetaViewModel
    {
        [Required(ErrorMessage = "É obrigatório informar o id do ponto de coleta que será editado.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do ponto de coleta é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O endereço do ponto de coleta é obrigatório.")]
        public string Endereco { get; set; }

        [Required(ErrorMessage = "A capacidade máxima em kg é obrigatória.")]
        public int CapacidadeMaximaKg { get; set; }
        public ICollection<int> PontosColetaResiduos { get; set; }
            = new List<int>();
    }
}
