using FeatureFlag.Dominio.Infra;
using MongoDB.Bson;

namespace FeatureFlag.Dominio;

public interface IRepRecursoConsumidor : IRepBase<RecursoConsumidor>
{
    Task<RecursoConsumidor?> RecuperarPorRecursoEConsumidorAsync(string identificadorRecurso, string identificadorConsumidor);
    Task<List<RecursoConsumidor>> RecuperarPorConsumidorAsync(ObjectId codigoConsumidor);
    Task<List<RecursoConsumidor>> RecuperarPorRecursoAsync(ObjectId codigoRecurso);
    Task<List<RecursoConsumidor>> RecuperarPorStatusAsync(EnumStatusRecursoConsumidor status);
}