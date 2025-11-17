namespace coleta_residuos.ViewModel
{
    public class AlertaViewModel
    {
        public int Id { get; set; }
        public int PontoColetaId { get; set; }
        public DateTime DataAlerta { get; set; }
        public string Mensagem { get; set; }
        public bool Resolvido { get; set; }
    }

}
