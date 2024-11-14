using FeatureFlag.Domain.Infra;
using FeatureFlag.Dominio.RecursoConsumidor.Dtos;

namespace FeatureFlag.Domain;

public interface IRepRecursoConsumidor : IRepBase<RecursoConsumidor> 
{
    Task<RecursoConsumidorResponse?> RecuperarPorRecursoConsumidorAsync(string descricaoRecurso, string identificadorConsumidor);
    IQueryable<RecursoConsumidorResponse> RecuperarPorConsumidor(string identificadorConsumidor);
}