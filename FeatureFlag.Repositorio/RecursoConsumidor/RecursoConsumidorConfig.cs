using FeatureFlag.Dominio;
using FeatureFlag.Repositorio.Infra;
using MongoDB.Bson.Serialization;

namespace FeatureFlag.Repositorio;

public class RecursoConsumidorConfig : EntidadeBaseConfig<RecursoConsumidor>
{
    public override void Configurar(BsonClassMap<RecursoConsumidor> builder)
    {
        base.Configurar(builder);
        
        builder.SetIsRootClass(false);

        builder.MapProperty(x => x.Recurso)
               .SetElementName("recurso");
        
        builder.MapProperty(x => x.Consumidor)
               .SetElementName("consumidor");

        builder.MapProperty(x => x.Status)
               .SetElementName("status");

        builder.MapProperty(x => x.Congelado)
               .SetElementName("congelado");
    }
}