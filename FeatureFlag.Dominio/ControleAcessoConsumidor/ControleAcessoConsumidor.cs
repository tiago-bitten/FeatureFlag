using FeatureFlag.Dominio.Infra;

namespace FeatureFlag.Dominio;

public sealed class ControleAcessoConsumidor : EntidadeBase
{
    public ConsumidorEmbedded Consumidor { get; private set; }
    public RecursoEmbedded Recurso { get; private set; }
    public EnumTipoControle Tipo { get; private set; }

    #region Setters
    public void DefinirWhitelist() => Tipo = EnumTipoControle.Whitelist;
    public void DefinirBlacklist() => Tipo = EnumTipoControle.Blacklist;
    #endregion

    #region Fábrica Estática
    private ControleAcessoConsumidor(string codigoConsumidor, string codigoRecurso, EnumTipoControle tipoControle)
    {
        Consumidor.Id = codigoConsumidor;
        Recurso.Id = codigoRecurso;
        Tipo = tipoControle;
    }

    public static ControleAcessoConsumidor CriarWhitelist(string codigoConsumidor, string codigoRecurso)
    {
        var controleAcessoConsumidor = new ControleAcessoConsumidor(codigoConsumidor, codigoRecurso, EnumTipoControle.Whitelist);
        return controleAcessoConsumidor;
    }

    public static ControleAcessoConsumidor CriarBlacklist(string codigoConsumidor, string codigoRecurso)
    {
        var controleAcessoConsumidor = new ControleAcessoConsumidor(codigoConsumidor, codigoRecurso, EnumTipoControle.Blacklist);
        return controleAcessoConsumidor;
    }
    #endregion
    
    #region Embeddeds

    #region ConsumidorEmbedded
    public class ConsumidorEmbedded
    {
        public string Id { get; set; }
        public string Identificador { get; set; }
    }
    #endregion

    #region RecursoEmbedded
    public class RecursoEmbedded
    {
        public string Id { get; set; }
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