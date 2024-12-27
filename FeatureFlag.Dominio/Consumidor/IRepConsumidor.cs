using FeatureFlag.Dominio.Infra;
using MongoDB.Bson;

namespace FeatureFlag.Dominio;

public interface IRepConsumidor : IRepBase<Consumidor>
{
    Task<Consumidor?> RecuperarPorIdentificadorAsync(string identificador);
    Task<bool> ExistePorIdentificadorAsync(string identificador);
    Task<List<Consumidor>> RecuperarPorRecursoAsync(ObjectId codigoRecurso);
}