using MongoDB.Bson.Serialization;

namespace FeatureFlag.Repositorio.Infra;

public interface IDocumentConfiguration<T>
{
    void Configurar(BsonClassMap<T> builder);
}