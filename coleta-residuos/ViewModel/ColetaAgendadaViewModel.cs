namespace coleta_residuos.ViewModel
{
    public class ColetaAgendadaViewModel
    {
        public int Id { get; set; }
        public DateTime DataAgendada { get; set; }
        public string? Observacao { get; set; }
        public int PontoColetaId { get; set; }
    }

}
