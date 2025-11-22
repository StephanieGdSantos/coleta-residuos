namespace coleta_residuos.ViewModel
{
    public class PontoColetaViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public int CapacidadeMaximaKg { get; set; }
        public List<ResiduoViewModel> Residuos { get; set; } = new();
    }
}
