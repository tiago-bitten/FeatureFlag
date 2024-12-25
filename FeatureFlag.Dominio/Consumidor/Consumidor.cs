using FeatureFlag.Domain;
using FeatureFlag.Dominio.Infra;

namespace FeatureFlag.Dominio;

public sealed class Consumidor : EntidadeBase
{
    public string Identificador { get; private set; }
    public string? Descricao { get; private set; }
    public List<RecursoConsumidorEmbedded> Recursos { get; private set; } = [];
    public List<ControleAcessoConsumidorEmbedded> ControleAcessos { get; private set; } = [];
    
    #region Regras
    public void ValidarIdentificador()
    {
        if (string.IsNullOrWhiteSpace(Identificador))
            throw new Exception("Identificador é obrigatório");
    }
    #endregion
    
    #region Setters
    #region AdicionarRecursoHabilitado
    public void AdicionarRecursoHabilitado(string identificadorRecurso)
    {
        var recursoConsumidor = new RecursoConsumidorEmbedded();
        recursoConsumidor.AdicioniarHabilitado(identificadorRecurso);
        Recursos.Add(recursoConsumidor);
    }
    #endregion
    
    #region AdicionarRecursoDesabilitado
    public void AdicionarRecursoDesabilitado(string identificadorRecurso)
    {
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
    #endregion
    
    #region Fábrica estática
    private Consumidor(string identificador, string? descricao = null)
    {
        Identificador = identificador;
        Descricao = descricao;
        ValidarIdentificador();
    }
    
    public static Consumidor Criar(string identificador, string? descricao = null)
    {
        var consumidor = new Consumidor(identificador, descricao);
        return consumidor;
    }
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
        
        public void AdicioniarDesabilitado(string identificadorRecurso)
        {
            IdentificadorRecurso = identificadorRecurso;
            Status = EnumStatusRecursoConsumidor.Desabilitado;
        }
    }
    #endregion

    #region ControleAcessoConsumidorEmbedded
    public class ControleAcessoConsumidorEmbedded
    {
        public string IdentificadorRecurso { get; set; }
        public EnumTipoControle Tipo { get; set; }
    }
    #endregion
    #endregion
}