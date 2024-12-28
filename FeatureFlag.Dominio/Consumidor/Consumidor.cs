using FeatureFlag.Domain;
using FeatureFlag.Dominio.Infra;
using FeatureFlag.Shared.ValueObjects;
using MongoDB.Bson;

namespace FeatureFlag.Dominio;

public sealed class Consumidor : EntidadeBase
{
    public Identificador Identificador { get; private set; }
    public string? Descricao { get; private set; }
    public List<ControleAcessoConsumidorEmbedded> ControleAcessos { get; private set; } = [];
    public bool PossuiControleAcessoPorRecurso(ObjectId codigoRecurso) => ControleAcessos.Any(x => x.Recurso.Id == codigoRecurso);

    #region Ctor
    public Consumidor(string identificador)
    {
        Identificador = new Identificador(identificador);
    }
    #endregion
    
    #region Setters
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