using coleta_residuos.Models;

namespace coleta_residuos.ViewModel
{
    public class AtualizarPontoColetaViewModel
    {
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public int CapacidadeMaximaKg { get; set; }
        public List<int> PontosColetaResiduos { get; set; }
            = new();
        public ICollection<int> EventosColeta { get; set; }
            = new List<int>();
    }
}
