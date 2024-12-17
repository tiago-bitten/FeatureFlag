using FeatureFlag.Repositorio.Infra;
using MongoDB.Bson.Serialization;

namespace FeatureFlag.Repositorio;

public class RecursoConsumidorConfig : EntidadeBaseConfig<Dominio.RecursoConsumidor>
{
    public override void Configurar(BsonClassMap<Dominio.RecursoConsumidor> builder)
    {
        base.Configurar(builder);

        builder.MapProperty(x => x.Recurso)
            .SetElementName("recurso");
        
        builder.MapProperty(x => x.Consumidor)
            .SetElementName("consumidor");
    }
}