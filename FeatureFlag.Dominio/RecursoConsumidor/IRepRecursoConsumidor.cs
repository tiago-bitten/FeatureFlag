using FeatureFlag.Dominio.Infra;
using FeatureFlag.Dominio.Dtos;

namespace FeatureFlag.Dominio;

public interface IRepRecursoConsumidor : IRepBase<RecursoConsumidor> 
{
    Task<RecursoConsumidor?> RecuperarPorRecursoConsumidorAsync(string descricaoRecurso, string identificadorConsumidor, params string[]? includes);
    IQueryable<RecursoConsumidor> RecuperarPorConsumidor(string identificadorConsumidor, params string[]? includes);
    IQueryable<RecursoConsumidor> RecuperarPorRecurso(string identificadorRecurso, params string[]? includes);
    IQueryable<RecursoConsumidor> RecuperarPorStatus(EnumStatusRecursoConsumidor status, params string[]? includes);
    IQueryable<RecursoConsumidor> RecuperarPorTipo(string identificadorRecurso, EnumTipoControle tipo, params string[]? includes);
}