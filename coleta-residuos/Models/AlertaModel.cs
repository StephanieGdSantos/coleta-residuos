namespace coleta_residuos.Models
{
    public class AlertaModel
    {
        public int Id { get; set; }
        public int PontoColetaId { get; set; }
        public PontoColetaModel PontoColeta { get; set; }
        public DateTime DataAlerta { get; set; }
        public string Mensagem { get; set; }
        public bool Resolvido { get; set; }
    }
}
