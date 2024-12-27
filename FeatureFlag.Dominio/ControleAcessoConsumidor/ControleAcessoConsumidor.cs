using FeatureFlag.Domain;
using FeatureFlag.Dominio.Infra;
using MongoDB.Bson;

namespace FeatureFlag.Dominio;

public sealed class ControleAcessoConsumidor : EntidadeBase
{
    public ConsumidorEmbedded Consumidor { get; private set; } = new();
    public RecursoEmbedded Recurso { get; private set; } = new();
    public EnumTipoControle Tipo { get; private set; }

    public ControleAcessoConsumidor(Consumidor consumidor, Recurso recurso, EnumTipoControle tipo)
    {
        Consumidor = new ConsumidorEmbedded
        {
            Id = consumidor.Id,
            Identificador = consumidor.Identificador
        };
        
        Recurso = new RecursoEmbedded
        {
            Id = recurso.Id,
            Identificador = recurso.Identificador  
        };
        
        Tipo = tipo;
    }
    
    #region Setters
    public void DefinirWhitelist() => Tipo = EnumTipoControle.Whitelist;
    public void DefinirBlacklist() => Tipo = EnumTipoControle.Blacklist;
    #endregion

    #region Fábrica Estática
    private ControleAcessoConsumidor(ObjectId codigoConsumidor, ObjectId codigoRecurso, EnumTipoControle tipoControle)
    {
        Consumidor.Id = codigoConsumidor;
        Recurso.Id = codigoRecurso;
        Tipo = tipoControle;
    }

    public static ControleAcessoConsumidor CriarWhitelist(ObjectId codigoConsumidor, ObjectId codigoRecurso)
    {
        var controleAcessoConsumidor = new ControleAcessoConsumidor(codigoConsumidor, codigoRecurso, EnumTipoControle.Whitelist);
        return controleAcessoConsumidor;
    }

    public static ControleAcessoConsumidor CriarBlacklist(ObjectId codigoConsumidor, ObjectId codigoRecurso)
    {
        var controleAcessoConsumidor = new ControleAcessoConsumidor(codigoConsumidor, codigoRecurso, EnumTipoControle.Blacklist);
        return controleAcessoConsumidor;
    }
    #endregion
    
    #region Embeddeds

    #region ConsumidorEmbedded
    public class ConsumidorEmbedded
    {
        public ObjectId Id { get; set; }
        public string Identificador { get; set; }
    }
    #endregion

    #region RecursoEmbedded
    public class RecursoEmbedded
    {
        public ObjectId Id { get; set; }
        public string Identificador { get; set; }
    }
    #endregion
    #endregion
}

public enum EnumTipoControle
{
    Whitelist = 1,
    Blacklist = 2
}