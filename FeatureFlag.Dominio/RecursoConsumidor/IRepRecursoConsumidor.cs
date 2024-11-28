using FeatureFlag.Dominio.Infra;
using FeatureFlag.Dominio.Dtos;

namespace FeatureFlag.Dominio;

public interface IRepRecursoConsumidor : IRepBase<RecursoConsumidor> 
{
    Task<RecursoConsumidor?> RecuperarPorRecursoConsumidorAsync(string descricaoRecurso, string identificadorConsumidor);
    IQueryable<RecursoConsumidor> RecuperarPorConsumidor(string identificadorConsumidor);
    IQueryable<RecursoConsumidor> RecuperarPorRecurso(string identificadorRecurso);
    IQueryable<RecursoConsumidor> RecuperarHabilitadosPorRecurso(string identificadorRecurso);
    IQueryable<RecursoConsumidor> RecuperarDesabilitadosPorRecurso(string identificadorRecurso);
    IQueryable<RecursoConsumidor> RecuperarPorTipo(string identificadorRecurso, EnumTipoControle tipo);
}