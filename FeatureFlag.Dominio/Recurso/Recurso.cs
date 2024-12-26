using FeatureFlag.Dominio.Infra;
using FeatureFlag.Shared.Helpers;

namespace FeatureFlag.Domain;

public sealed class Recurso : EntidadeBase
{
    public string Identificador { get; private set; }
    public string Descricao { get; set; }
    public PorcentagemEmbedded Porcentagem { get; private set; } = new();
    public ConsumidorEmbedded Consumidor { get; private set; } = new();
    
    #region Setters
    public void AlterarIdentificador(string novoIdentificador)
    {
        Identificador = novoIdentificador;
        ValidarIdentificador();
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
    #endregion
    
    #region Fábrica estática
    private Recurso(string identificador, string descricao, decimal porcentagem)
    {
        Identificador = identificador;
        Descricao = descricao;
        Porcentagem.Atualizar(porcentagem);
    }
    
    public static Recurso Criar(string identificador, string descricao, decimal porcentagem)
    {
        var recurso = new Recurso(identificador, descricao, porcentagem);
        
        recurso.ValidarIdentificador();
        recurso.ValidarDescricao();
        
        return recurso;
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
    
    #region PorcentagemEmbedded
    public class PorcentagemEmbedded
    {
        public decimal Alvo { get; private set; }
        public bool Atingido { get; private set; }
        public decimal ValorAtingido { get; private set; }

        #region ValidarAlvo
        public void ValidarAlvo(decimal alvo)
        {
            if (!PorcentagemHelper.Validar(alvo))
                throw new Exception("A porcentagem deve ser entre 0 e 100");
        }
        #endregion

        #region Atualizar
        public void Atualizar(decimal novoAlvo)
        {
            ValidarAlvo(novoAlvo);
            Alvo = novoAlvo;
            Resetar();
        }
        #endregion
        
        #region Atingir
        public void  Atingir(decimal valor)
        {
            ValorAtingido = valor;
            Atingido = true;
        }
        #endregion
        
        #region Resetar
        public void Resetar()
        {
            Atingido = false;
            ValorAtingido = 0;
        }
        #endregion
    }
    #endregion
    #endregion  
}