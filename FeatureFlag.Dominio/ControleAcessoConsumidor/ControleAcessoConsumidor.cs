using FeatureFlag.Domain;
using FeatureFlag.Domain.Infra;

namespace FeatureFlag.Dominio;

public sealed class ControleAcessoConsumidor : EntidadeBase
{
    public Guid CodigoConsumidor { get; private set; }
    public Guid CodigoRecurso { get; private set; }
    public EnumTipoControle Tipo { get; private set; }

    #region Relacionamentos
    public Consumidor Consumidor { get; private set; }
    public Recurso Recurso { get; private set; }
    #endregion
    
    #region Regras

    #region ValidarCodigos
    public void ValidarCodigos()
    {
        if (CodigoConsumidor == Guid.Empty || CodigoRecurso == Guid.Empty)
            throw new Exception("Código do Consumidor ou Recurso é inválido.");
    }
    #endregion

    #endregion
    
    #region Tipo de Controle
    public void DefinirWhitelist() => Tipo = EnumTipoControle.Whitelist;
    public void DefinirBlacklist() => Tipo = EnumTipoControle.Blacklist;
    #endregion

    #region Fábrica Estática
    private ControleAcessoConsumidor(Guid codigoConsumidor, Guid codigoRecurso, EnumTipoControle tipoControle)
    {
        CodigoConsumidor = codigoConsumidor;
        CodigoRecurso = codigoRecurso;
        Tipo = tipoControle;
        ValidarCodigos();
    }

    public static ControleAcessoConsumidor CriarWhitelist(Guid codigoConsumidor, Guid codigoRecurso)
    {
        return new ControleAcessoConsumidor(codigoConsumidor, codigoRecurso, EnumTipoControle.Whitelist);
    }

    public static ControleAcessoConsumidor CriarBlacklist(Guid codigoConsumidor, Guid codigoRecurso)
    {
        return new ControleAcessoConsumidor(codigoConsumidor, codigoRecurso, EnumTipoControle.Blacklist);
    }
    #endregion
}

public enum EnumTipoControle
{
    Whitelist = 1,
    Blacklist = 2
}