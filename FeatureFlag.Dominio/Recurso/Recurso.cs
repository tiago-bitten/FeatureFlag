using FeatureFlag.Domain.Infra;

namespace FeatureFlag.Domain;

public sealed class Recurso : EntidadeBase
{
    public string Identificador { get; private set; }
    public string? Descricao { get; private set; }
    
    #region Relacionamentos    
    public List<Consumidor> Consumidores { get; set; } = [];
    public List<RecursoConsumidor> RecursoConsumidores { get; set; } = [];
    #endregion
    
    #region Regras
    public void ValidarIdentificador()
    {
        if (string.IsNullOrWhiteSpace(Identificador))
            throw new Exception("Identificador é obrigatório");
    }
    
    public void ValidarDescricao()
    {
        if (string.IsNullOrWhiteSpace(Descricao))
            throw new Exception("Descrição é obrigatória");
    }
    #endregion
    
    #region Fábrica estática
    private Recurso(string identificador, string descricao)
    {
        Identificador = identificador;
        Descricao = descricao;
    }
    
    public static Recurso Criar(string identificador, string descricao)
    {
        var recurso = new Recurso(identificador, descricao);
        
        recurso.ValidarIdentificador();
        recurso.ValidarDescricao();
        
        return recurso;
    }
    #endregion
}