using System.Drawing;

namespace coleta_residuos.Models
{
    public class ResiduoModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Categoria { get; set; }
        public string Descricao { get; set; }
        public ICollection<PontoColetaResiduoModel> PontosColetaResiduos { get; set; }
            = new List<PontoColetaResiduoModel>();
    }
}

