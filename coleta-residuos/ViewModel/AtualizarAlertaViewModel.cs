namespace coleta_residuos.ViewModel
{
    public class AtualizarAlertaViewModel
    {
        public int Id { get; set; }
        public int PontoColetaId { get; set; }
        public string Mensagem { get; set; }
        public bool Resolvido { get; set; }
    }
}
