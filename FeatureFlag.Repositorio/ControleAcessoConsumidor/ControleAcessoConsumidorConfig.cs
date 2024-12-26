using FeatureFlag.Dominio;
using FeatureFlag.Repositorio.Infra;
using MongoDB.Bson.Serialization;

namespace FeatureFlag.Repositorio;

public class ControleAcessoConsumidorConfig : EntidadeBaseConfig<ControleAcessoConsumidor>
{
    public override void Configurar(BsonClassMap<ControleAcessoConsumidor> builder)
    {
        base.Configurar(builder);

        builder.MapProperty(x => x.Consumidor) 
               .SetElementName("consumidor");
        
        builder.MapProperty(x => x.Recurso) 
               .SetElementName("recurso");

        builder.MapProperty(x => x.Tipo)
               .SetElementName("tipo");
    }
}