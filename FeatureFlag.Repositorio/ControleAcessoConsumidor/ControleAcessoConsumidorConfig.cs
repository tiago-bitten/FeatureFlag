using FeatureFlag.Dominio;
using FeatureFlag.Repositorio.Infra;
using MongoDB.Bson.Serialization;

namespace FeatureFlag.Repositorio;

public class ControleAcessoConsumidorConfig : EntidadeBaseConfig<ControleAcessoConsumidor>
{
    public override void Configurar(BsonClassMap<ControleAcessoConsumidor> builder)
    {
        base.Configurar(builder);
        
        
    }
}