using FeatureFlag.Dominio.Infra;

namespace FeatureFlag.Dominio;

public interface IRepRecursoConsumidor : IRepBase<RecursoConsumidor>
{
    Task<RecursoConsumidor?> RecuperarPorRecursoEConsumidorAsync(string codigoRecurso, string codigoConsumidor);
    Task<List<RecursoConsumidor>> RecuperarPorConsumidorAsync(string codigoConsumidor);
    Task<List<RecursoConsumidor>> RecuperarPorRecursoAsync(string codigoRecurso);
    Task<List<RecursoConsumidor>> RecuperarPorStatusAsync(EnumStatusRecursoConsumidor status);
}