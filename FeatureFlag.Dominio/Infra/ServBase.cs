namespace FeatureFlag.Dominio.Infra;

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

    #region AdicionarAsync
    public async Task AdicionarAsync(TEntidade entidade)
    {
        await Repositorio.AdicionarAsync(entidade);
    }
    #endregion
    
    #region AlterarAsync
    public async Task AlterarAsync(TEntidade entidade)
    {
        entidade.Alterar();
        await Repositorio.AtualizarAsync(entidade);
    }
    #endregion
}