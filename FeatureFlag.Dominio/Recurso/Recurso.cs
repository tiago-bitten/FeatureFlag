using FeatureFlag.Dominio.Infra;
using FeatureFlag.Shared.Helpers;

namespace FeatureFlag.Domain;

public sealed class Recurso : EntidadeBase
{
    public string Identificador { get; private set; }
    public string Descricao { get; set; }
    public decimal Porcentagem { get; private set; }
    
    public ConsumidorEmbedded Consumidor { get; private set; } = new();
    
    #region Setters
    public void AlterarPorcentagem(decimal porcentagem)
    {
        Porcentagem = porcentagem;
        ValidarPorcentagem();
    }
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

    public void ValidarPorcentagem()
    {
        if (!PorcentagemHelper.Validar(Porcentagem))
            throw new Exception("A porcentagem deve ser entre 0 e 100");
    }
    #endregion
    
    #region Fábrica estática
    private Recurso(string identificador, string descricao, decimal porcentagem)
    {
        Identificador = identificador;
        Descricao = descricao;
        Porcentagem = porcentagem;
    }
    
    public static Recurso Criar(string identificador, string descricao, decimal porcentagem)
    {
        var recurso = new Recurso(identificador, descricao, porcentagem);
        
        recurso.ValidarIdentificador();
        recurso.ValidarDescricao();
        recurso.ValidarPorcentagem();
        
        return recurso;
    }
    #endregion
}

#region Embeddeds
public class ConsumidorEmbedded
{
    public int TotalHabilitados { get; private set; }
    public List<string> IdentificadoresHabilitados { get; private set; } = [];
    
    #region Adicionar
    public void Adicionar(string identificador)
    {
        IdentificadoresHabilitados.Add(identificador);
        TotalHabilitados++;
    }
    
    #endregion
    
    #region Remover
    public void Remover(string identificador)
    {
        IdentificadoresHabilitados.Remove(identificador);
        TotalHabilitados--;
    }
    #endregion
}
#endregion