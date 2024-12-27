using FeatureFlag.Dominio.Infra;
using FeatureFlag.Shared.Helpers;
using FeatureFlag.Shared.ValueObjects;

namespace FeatureFlag.Domain;

public sealed class Recurso : EntidadeBase
{
    public Identificador Identificador { get; private set; }
    public string Descricao { get; private set; }
    public decimal Porcentagem { get; private set; }
    public ConsumidorEmbedded Consumidor { get; private set; } = new();
    
    #region Ctor
    public Recurso(string identificador, string descricao, decimal porcentagem)
    {
        Identificador = new Identificador(identificador);
        Descricao = descricao;
        Porcentagem = porcentagem;
        
        ValidarDescricao();
        ValidarPorcentagem();
    }
    #endregion
    
    #region Setters
    public void AlterarIdentificador(string novoIdentificador)
    {
        Identificador = new Identificador(novoIdentificador);
    }
    #endregion
    
    #region Regras
    public void ValidarDescricao()
    {
        if (string.IsNullOrWhiteSpace(Descricao))
            ThrowHelper.FieldRequiredException("Descrição");
    }

    public void ValidarPorcentagem()
    {
        if (!PorcentagemHelper.Validar(Porcentagem))
            ThrowHelper.BusinessException("Porcentagem deve ser entre 0 e 100.");
    }
    #endregion
    
    #region Embeddeds
    #region ConsumidorEmbedded
    public class ConsumidorEmbedded
    {
        public int TotalHabilitados { get; private set; }
    
        #region Adicionar
        public void Adicionar() => TotalHabilitados++;
        #endregion
    
        #region Remover
        public void Remover() => TotalHabilitados--;
        #endregion
    }
    #endregion
    #endregion  
}