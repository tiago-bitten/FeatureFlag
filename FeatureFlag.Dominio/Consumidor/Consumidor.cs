using FeatureFlag.Domain;
using FeatureFlag.Dominio.Infra;
using FeatureFlag.Shared.ValueObjects;
using MongoDB.Bson;

namespace FeatureFlag.Dominio;

public sealed class Consumidor : EntidadeBase
{
    public Identificador Identificador { get; private set; }
    public string? Descricao { get; private set; }
    public List<RecursoConsumidorEmbedded> RecursoConsumidores { get; private set; } = [];
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
    public void AdicionarRecursoHabilitado(Recurso recurso)
    {
        RemoverRecurso(recurso.Identificador);
        var recursoConsumidor = new RecursoConsumidorEmbedded();
        recursoConsumidor.AdicioniarHabilitado(recurso);
        RecursoConsumidores.Add(recursoConsumidor);
    }
    #endregion
    
    #region AdicionarRecursoDesabilitado
    public void AdicionarRecursoDesabilitado(Recurso recurso)
    {
        RemoverRecurso(recurso.Identificador);
        var recursoConsumidor = new RecursoConsumidorEmbedded();
        recursoConsumidor.AdicioniarDesabilitado(recurso);
        RecursoConsumidores.Add(recursoConsumidor);
    }
    #endregion
    
    #region RemoverRecurso
    public void RemoverRecurso(string identificadorRecurso)
    {
        RecursoConsumidores.RemoveAll(x => x.Recurso.Identificador == identificadorRecurso);
    }
    #endregion
    
    #region AdicionarWhitelist
    public void AdicionarWhitelist(Recurso recurso)
    {
        RemoverControleAcesso(recurso.Identificador);
        var controleAcesso = new ControleAcessoConsumidorEmbedded();
        controleAcesso.AdicionarWhitelist(recurso);
        ControleAcessos.Add(controleAcesso);
    }
    #endregion
    
    #region AdicionarBlacklist
    public void AdicionarBlacklist(Recurso recurso)
    {
        RemoverControleAcesso(recurso.Identificador);
        var controleAcesso = new ControleAcessoConsumidorEmbedded();
        controleAcesso.AdicionarBlacklist(recurso);
        ControleAcessos.Add(controleAcesso);
    }
    #endregion
    
    #region RemoverControleAcesso
    public void RemoverControleAcesso(string identificadorRecurso)
    {
        ControleAcessos.RemoveAll(x => x.Recurso.Identificador == identificadorRecurso);
    }
    #endregion
    #endregion
    
    #region Embeddeds

    #region RecursoConsumidorEmbedded
    public class RecursoConsumidorEmbedded
    {
        public RecursoEmbedded Recurso { get; private set; }
        public EnumStatusRecursoConsumidor Status { get; set; }
        
        public void AdicioniarHabilitado(Recurso recurso)
        {
            Recurso = new RecursoEmbedded(recurso.Id, recurso.Identificador);
            Status = EnumStatusRecursoConsumidor.Habilitado;
        }
        
        public void  AdicioniarDesabilitado(Recurso recurso)
        {
            Recurso = new RecursoEmbedded(recurso.Id, recurso.Identificador);
            Status = EnumStatusRecursoConsumidor.Desabilitado;
        }
    }
    #endregion

    #region ControleAcessoConsumidorEmbedded
    public class ControleAcessoConsumidorEmbedded
    {
        public RecursoEmbedded Recurso { get; private set; }
        public EnumTipoControle Tipo { get; private set; }
        
        public void AdicionarWhitelist(Recurso recurso)
        {
            Recurso = new RecursoEmbedded(recurso.Id, recurso.Identificador);
            Tipo = EnumTipoControle.Whitelist;
        }
        
        public void AdicionarBlacklist(Recurso recurso)
        {
            Recurso = new RecursoEmbedded(recurso.Id, recurso.Identificador);
            Tipo = EnumTipoControle.Blacklist;
        }
    }
    #endregion
    
    #region RecursoEmbedded
    public class RecursoEmbedded
    {
        public ObjectId Id { get; init; }
        public string Identificador { get; set; }
        
        public RecursoEmbedded(ObjectId id, string identificador)
        {
            Id = id;
            Identificador = identificador;
        }
    }
    #endregion
    #endregion
}