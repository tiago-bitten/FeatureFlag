using FeatureFlag.Dominio;
using FeatureFlag.Repositorio.Infra;
using MongoDB.Bson.Serialization;

namespace FeatureFlag.Repositorio;

public class ConsumidorConfig : EntidadeBaseConfig<Consumidor>
{
    public override void Configurar(BsonClassMap<Consumidor> builder)
    {
        base.Configurar(builder);
        
        builder.SetIsRootClass(false);

        builder.MapProperty(x => x.Identificador)
               .SetElementName("identificador");

        builder.MapProperty(x => x.Descricao)
               .SetElementName("descricao");
        
        builder.MapProperty(x => x.Recursos)
               .SetElementName("recursos");
        
        builder.MapProperty(x => x.ControleAcessos)
               .SetElementName("controle_acessos");
    }
}