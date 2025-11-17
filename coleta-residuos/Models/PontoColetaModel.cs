namespace coleta_residuos.Models
{
    public class PontoColetaModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public int CapacidadeMaximaKg { get; set; }
        public ICollection<PontoColetaResiduoModel> PontosColetaResiduos { get; set; }
            = new List<PontoColetaResiduoModel>();
        public ICollection<EventoColetaModel> EventosColeta { get; set; }
            = new List<EventoColetaModel>();
    }
}

