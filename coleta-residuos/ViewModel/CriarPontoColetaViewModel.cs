namespace coleta_residuos.ViewModel
{
    public class CriarPontoColetaViewModel
    {
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public int CapacidadeMaximaKg { get; set; }
        public List<int> ResiduosIds { get; set; } = new List<int>();
    }
}