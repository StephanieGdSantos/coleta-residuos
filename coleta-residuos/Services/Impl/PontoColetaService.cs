using coleta_residuos.Data.Repository;
using coleta_residuos.Models;
using Microsoft.EntityFrameworkCore;

namespace coleta_residuos.Services.Impl
{
    public class PontoColetaService : IService<PontoColetaModel>
    {
        private readonly IRepository<PontoColetaModel> _repository;

        public PontoColetaService(IRepository<PontoColetaModel> repository)
        {
            _repository = repository;
        }

        public void Atualizar(PontoColetaModel pontoColeta)
        {
            var pontoColetaExistente = _repository.GetById(pontoColeta.Id);
            if (pontoColetaExistente == null)
                throw new Exception("Ponto de Coleta não encontrado.");

            pontoColetaExistente.Nome = pontoColeta.Nome;
            pontoColetaExistente.Endereco = pontoColeta.Endereco;
            pontoColetaExistente.CapacidadeMaximaKg = pontoColeta.CapacidadeMaximaKg;

            var residuosNovosIds = pontoColeta.PontosColetaResiduos.Select(r => r.ResiduoId).ToHashSet();
            var relacoesParaRemover = pontoColetaExistente.PontosColetaResiduos
                .Where(r => !residuosNovosIds.Contains(r.ResiduoId))
                .ToList();

            foreach (var relacao in relacoesParaRemover)
                pontoColetaExistente.PontosColetaResiduos.Remove(relacao);

            var residuosExistentesIds = pontoColetaExistente.PontosColetaResiduos.Select(r => r.ResiduoId).ToHashSet();
            var relacoesParaAdicionar = residuosNovosIds
                .Where(id => !residuosExistentesIds.Contains(id))
                .Select(residuoId => new PontoColetaResiduoModel
                {
                    PontoColetaId = pontoColeta.Id,
                    ResiduoId = residuoId
                });

            foreach (var novaRelacao in relacoesParaAdicionar)
                pontoColetaExistente.PontosColetaResiduos.Add(novaRelacao);

            _repository.Update(pontoColetaExistente);
        }


        public void Criar(PontoColetaModel pontoColeta) => _repository.Add(pontoColeta);


        public void Deletar(int id)
        {
            var pontoColeta = _repository.GetById(id);
            if (pontoColeta != null)
            {
                _repository.Delete(pontoColeta);
            }
        }

        public IEnumerable<PontoColetaModel> Listar(int pagina = 0, int tamanho = 10)
        {
            return _repository
                .GetAll(pagina, tamanho);
        }

        public PontoColetaModel ObterPorId(int id)
        {
            return _repository
                .GetById(id);
        }
    }
}
