namespace FeatureFlag.Domain.Infra;

public abstract class ServBase<TEntidade, TRepositorio> : IServBase<TEntidade, TRepositorio>
    where TEntidade : EntidadeBase
    where TRepositorio : IRepBase<TEntidade>
{
    #region Ctor
    public TRepositorio Repositorio { get; }

    public ServBase(TRepositorio repositorio)
    {
        Repositorio = repositorio;
    }
    #endregion

    #region InserirAsync
    public async Task AdicionarAsync(TEntidade entidade)
    {
        await Repositorio.AdicionarAsync(entidade);
    }
    #endregion
}