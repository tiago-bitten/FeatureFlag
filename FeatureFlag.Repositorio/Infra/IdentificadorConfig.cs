using FeatureFlag.Dominio.Infra;
using MongoDB.Bson.Serialization;

namespace FeatureFlag.Repositorio.Infra;

public class IdentificadorConfig<T> : IDocumentConfiguration<T> where T : IdentificadorObjectId
{
    public virtual void Configurar(BsonClassMap<T> builder)
    {
        builder.SetIgnoreExtraElements(true);
        
        builder.SetIsRootClass(true);
        
        if (typeof(T).IsAssignableFrom(typeof(IdentificadorObjectId)))
        {
            builder.MapIdProperty(x => x.Id)
                   .SetIdGenerator(MongoDB.Bson.Serialization.IdGenerators.ObjectIdGenerator.Instance);
        }
    }
}