using FeatureFlag.Domain;
using FeatureFlag.Repositorio.Infra;
using MongoDB.Bson.Serialization;

namespace FeatureFlag.Repositorio;

public class RecursoConfig : IDocumentConfiguration<Recurso>
{
    public void Configurar(BsonClassMap<Recurso> builder)
    {
        builder.AutoMap();
    }
}