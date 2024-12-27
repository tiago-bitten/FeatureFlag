using FeatureFlag.Domain;
using FeatureFlag.Dominio.Infra;
using FeatureFlag.Shared.ValueObjects;

namespace FeatureFlag.Dominio;

public sealed class Consumidor : EntidadeBase
{
    public Identificador Identificador { get; private set; }
    public string? Descricao { get; private set; }
    public List<RecursoConsumidorEmbedded> Recursos { get; private set; } = [];
    public List<ControleAcessoConsumidorEmbedded> ControleAcessos { get; private set; } = [];

    #region Ctor
    public Consumidor(string identificador)
    {
        Identificador = new Identificador(identificador);
    }
    #endregion
    
    #region Setters
    #region AlterarDados
    public void AlterarDados(string identificador, string? descricao)
    {
        Identificador = new Identificador(identificador);
        Descricao = descricao;
    }
    #endregion
    
    #region AdicionarRecursoHabilitado
    public void AdicionarRecursoHabilitado(string identificadorRecurso)
    {
        RemoverRecurso(identificadorRecurso);
        var recursoConsumidor = new RecursoConsumidorEmbedded();
        recursoConsumidor.AdicioniarHabilitado(identificadorRecurso);
        Recursos.Add(recursoConsumidor);
    }
    #endregion
    
    #region AdicionarRecursoDesabilitado
    public void AdicionarRecursoDesabilitado(string identificadorRecurso)
    {
        RemoverRecurso(identificadorRecurso);
        var recursoConsumidor = new RecursoConsumidorEmbedded();
        recursoConsumidor.AdicioniarDesabilitado(identificadorRecurso);
        Recursos.Add(recursoConsumidor);
    }
    #endregion
    
    #region RemoverRecurso
    public void RemoverRecurso(string identificadorRecurso)
    {
        Recursos.RemoveAll(x => x.IdentificadorRecurso == identificadorRecurso);
    }
    #endregion
    
    #region AdicionarWhitelist
    public void AdicionarWhitelist(string identificadorRecurso)
    {
        RemoverControleAcesso(identificadorRecurso);
        var controleAcesso = new ControleAcessoConsumidorEmbedded();
        controleAcesso.AdicionarWhitelist(identificadorRecurso);
        ControleAcessos.Add(controleAcesso);
    }
    #endregion
    
    #region AdicionarBlacklist
    public void AdicionarBlacklist(string identificadorRecurso)
    {
        RemoverControleAcesso(identificadorRecurso);
        var controleAcesso = new ControleAcessoConsumidorEmbedded();
        controleAcesso.AdicionarBlacklist(identificadorRecurso);
        ControleAcessos.Add(controleAcesso);
    }
    #endregion
    
    #region RemoverControleAcesso
    public void RemoverControleAcesso(string identificadorRecurso)
    {
        ControleAcessos.RemoveAll(x => x.IdentificadorRecurso == identificadorRecurso);
    }
    #endregion
    #endregion
    
    #region Embeddeds

    #region RecursoConsumidorEmbedded
    public class RecursoConsumidorEmbedded
    {
        public string IdentificadorRecurso { get; private set; }
        public EnumStatusRecursoConsumidor Status { get; private set; }
        
        public void AdicioniarHabilitado(string identificadorRecurso)
        {
            IdentificadorRecurso = identificadorRecurso;
            Status = EnumStatusRecursoConsumidor.Habilitado;
        }
        
        public void  AdicioniarDesabilitado(string identificadorRecurso)
        {
            IdentificadorRecurso = identificadorRecurso;
            Status = EnumStatusRecursoConsumidor.Desabilitado;
        }
    }
    #endregion

    #region ControleAcessoConsumidorEmbedded
    public class ControleAcessoConsumidorEmbedded
    {
        public string IdentificadorRecurso { get; private set; }
        public EnumTipoControle Tipo { get; private set; }
        
        public void AdicionarWhitelist(string identificadorRecurso)
        {
            IdentificadorRecurso = identificadorRecurso;
            Tipo = EnumTipoControle.Whitelist;
        }
        
        public void AdicionarBlacklist(string identificadorRecurso)
        {
            IdentificadorRecurso = identificadorRecurso;
            Tipo = EnumTipoControle.Blacklist;
        }
    }
    #endregion
    #endregion
}