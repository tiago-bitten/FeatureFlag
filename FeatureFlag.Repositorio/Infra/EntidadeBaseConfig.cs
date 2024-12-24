using FeatureFlag.Dominio.Infra;
using MongoDB.Bson.Serialization;

namespace FeatureFlag.Repositorio.Infra;

public class EntidadeBaseConfig<T> : IdentificadorConfig<T> where T : EntidadeBase
{
    public override void Configurar(BsonClassMap<T> builder)
    {
        base.Configurar(builder);

        if (typeof(T) != typeof(EntidadeBase)) return;
        
        builder.MapProperty(x => x.DataCriacao)
               .SetElementName("data_criacao");
        
        builder.MapProperty(x => x.DataAlteracao)
               .SetElementName("data_alteracao");
        
        builder.MapProperty(x => x.Inativo)
               .SetElementName("inativo");
    }
}