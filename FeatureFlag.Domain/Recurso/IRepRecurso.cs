using FeatureFlag.Domain.Infra;

namespace FeatureFlag.Domain;

public interface IRepRecurso : IRepBase<Recurso>
{
    Task<Recurso> RecuperarRecursoConsumidor(string descricao, string identificadorConsumidor);
}