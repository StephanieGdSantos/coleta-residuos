using System.ComponentModel.DataAnnotations;

namespace coleta_residuos.ViewModel
{
    public class CriarPontoColetaViewModel
    {
        [Required(ErrorMessage = "O ponto de coleta deve ter um nome.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O ponto de coleta deve ter um endereço.")]
        public string Endereco { get; set; }
        [Required]
        [Range(1, 10000, ErrorMessage = "A capacidade máxima deve ser entre 1 e 10.000 kg.")]
        public int CapacidadeMaximaKg { get; set; }
        public List<int> ResiduosIds { get; set; } = new List<int>();
    }
}