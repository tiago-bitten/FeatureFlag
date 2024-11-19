using FeatureFlag.Dominio.Infra;
using FeatureFlag.Dominio;
using FeatureFlag.Shared.Helpers;

namespace FeatureFlag.Domain;

public sealed class Recurso : EntidadeBase
{
    public string Identificador { get; private set; }
    public string Descricao { get; private set; }
    public decimal Porcentagem { get; private set; }
    
    #region Relacionamentos
    private readonly List<Consumidor> _consumidores = [];
    private readonly List<RecursoConsumidor> _recursoConsumidores = [];
    private readonly List<ControleAcessoConsumidor> _controleAcessoConsumidores = [];

    public IReadOnlyList<Consumidor> Consumidores => _consumidores.AsReadOnly();
    public IReadOnlyList<RecursoConsumidor> RecursoConsumidores => _recursoConsumidores.AsReadOnly();
    public IReadOnlyList<ControleAcessoConsumidor> ControleAcessoConsumidores => _controleAcessoConsumidores.AsReadOnly();
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