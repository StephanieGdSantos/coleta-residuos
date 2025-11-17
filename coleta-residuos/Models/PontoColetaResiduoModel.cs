using coleta_residuos.Data.Repository;
using coleta_residuos.Models;

namespace coleta_residuos.Models
{
    public class PontoColetaResiduoModel
    {
        public int ResiduoId { get; set; }
        public ResiduoModel Residuo { get; set; }

        public int PontoColetaId { get; set; }
        public PontoColetaModel PontoColeta { get; set; }
    }

}
