using FeatureFlag.Dominio.Infra;
using MongoDB.Bson.Serialization;

namespace FeatureFlag.Repositorio.Infra;

public class IdentificadorConfig<T> : IDocumentConfiguration<T> where T : Identificador
{
    public virtual void Configurar(BsonClassMap<T> builder)
    {
        builder.SetIgnoreExtraElements(true);
        
        builder.SetIsRootClass(true);
        
        if (typeof(T).IsAssignableFrom(typeof(Identificador)))
        {
            builder.MapIdProperty(x => x.Id)
                   .SetIdGenerator(MongoDB.Bson.Serialization.IdGenerators.ObjectIdGenerator.Instance);
        }
    }
}