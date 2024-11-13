namespace FeatureFlag.Domain.Infra;

public abstract class EntidadeBase : Identificador
{
    public DateTime DataCriacao { get; private set; } = DateTime.Now;
    public DateTime DataAlteracao { get; private set; } = DateTime.Now;
    public bool Inativo { get; private set; } = false;
    
    #region Métodos
    public void Alterar() => DataAlteracao = DateTime.Now;
    public void Inativar() => Inativo = true;
    #endregion
}