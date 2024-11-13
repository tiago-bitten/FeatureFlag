namespace FeatureFlag.Domain.Infra;

public abstract class ServBase<TEntidade, TRepositorio> : IServBase<TEntidade, TRepositorio>
    where TEntidade : EntidadeBase
    where TRepositorio : IRepBase<TEntidade>
{
    #region Ctor
    protected TRepositorio Repositorio;
    
    public ServBase(TRepositorio repositorio)
    {
        Repositorio = repositorio;
    }
    #endregion
}