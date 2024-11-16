namespace FeatureFlag.Dominio.RecursoConsumidor.Fabricas;

public interface IFabricaAlgoritmosAtualizacao
{
    void AlgoritmoVitao(string identificadorRecurso);
    Task AlgoritmoFisherYates(string identificadorRecurso, int quantidadeAlvo);
}