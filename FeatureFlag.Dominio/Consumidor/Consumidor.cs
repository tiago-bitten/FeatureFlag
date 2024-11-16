using FeatureFlag.Domain.Infra;
using FeatureFlag.Dominio;

namespace FeatureFlag.Domain;

public sealed class Consumidor : EntidadeBase
{
    public string Identificador { get; private set; }
    public string? Descricao { get; private set; }
    
    #region Relacionamentos
    private List<Recurso> _recursos = [];
    private List<RecursoConsumidor> _recursoConsumidores = [];
    private List<ControleAcessoConsumidor> _controleAcessoConsumidores = [];

    public IReadOnlyList<Recurso> Recursos => _recursos.AsReadOnly();
    public IReadOnlyList<RecursoConsumidor> RecursoConsumidores => _recursoConsumidores.AsReadOnly();
    public IReadOnlyList<ControleAcessoConsumidor> ControleAcessoConsumidores => _controleAcessoConsumidores.AsReadOnly();
    #endregion
    
    #region Regras
    public void ValidarIdentificador()
    {
        if (string.IsNullOrWhiteSpace(Identificador))
            throw new Exception("Identificador é obrigatório");
    }
    #endregion
    
    #region Fábrica estática
    private Consumidor(string identificador, string? descricao = null)
    {
        Identificador = identificador;
        Descricao = descricao;
    }
    
    public static Consumidor Criar(string identificador, string? descricao = null)
    {
        var consumidor = new Consumidor(identificador, descricao);
        consumidor.ValidarIdentificador();
        return consumidor;
    }
    #endregion
}