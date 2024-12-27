using FeatureFlag.Domain;
using FeatureFlag.Repositorio.Infra;
using FeatureFlag.Shared.Serializers;
using MongoDB.Bson.Serialization;

namespace FeatureFlag.Repositorio;

public class RecursoConfig : EntidadeBaseConfig<Recurso>
{
    public override void Configurar(BsonClassMap<Recurso> builder)
    {
        base.Configurar(builder);
        
        builder.SetIsRootClass(false);

        builder.MapProperty(x => x.Identificador)
            .SetElementName("identificador")
            .SetSerializer(new IdentificadorSerializer());
        
        builder.MapProperty(x => x.Descricao)
            .SetElementName("descricao");
        
        builder.MapProperty(x => x.Porcentagem)
            .SetElementName("porcentagem");
        
        builder.MapProperty(x => x.Consumidor)
            .SetElementName("consumidor");
    }
}