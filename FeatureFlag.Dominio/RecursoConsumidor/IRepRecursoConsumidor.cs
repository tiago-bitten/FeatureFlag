using FeatureFlag.Domain.Infra;

namespace FeatureFlag.Domain;

public interface IRepRecursoConsumidor : IRepBase<RecursoConsumidor> 
{
    Task<RecursoConsumidor?> RecuperarPorRecursoConsumidorAsync(string descricaoRecurso, string identificadorConsumidor);
    IQueryable<RecursoConsumidor> RecuperarPorConsumidor(string identificadorConsumidor);
}