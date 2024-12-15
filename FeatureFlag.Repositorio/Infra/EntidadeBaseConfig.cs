using FeatureFlag.Dominio.Infra;
using MongoDB.Bson.Serialization;

namespace FeatureFlag.Repositorio.Infra;

public class EntidadeBaseConfig<T> : IdentificadorConfig<T> where T : EntidadeBase
{
    public override void Configurar(BsonClassMap<T> builder)
    {
        base.Configurar(builder);
        
        builder.MapProperty(x => x.DataCriacao)
            .SetElementName("datacriacao");
        
        builder.MapProperty(x => x.DataAlteracao)
            .SetElementName("dataalteracao");
        
        builder.MapProperty(x => x.Inativo)
            .SetElementName("inativo");
    }
}