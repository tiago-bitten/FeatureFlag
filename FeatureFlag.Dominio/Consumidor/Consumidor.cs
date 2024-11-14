using FeatureFlag.Domain.Infra;

namespace FeatureFlag.Domain;

public sealed class Consumidor : EntidadeBase
{
    public string Identificador { get; private set; }
    public string? Descricao { get; private set; }
    
    #region Relacionamentos
    public List<Recurso> Recursos { get; set; } = [];
    public List<RecursoConsumidor> RecursoConsumidores { get; set; } = [];
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