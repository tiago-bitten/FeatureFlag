using FeatureFlag.Domain.Infra;

namespace FeatureFlag.Aplicacao.Infra;

public abstract class AplicBase<TEntidade, TRepo>
    where TEntidade : EntidadeBase
    where TRepo : IRepBase<TEntidade>
{
    protected TRepo Repositorio;
    
    public AplicBase(TRepo repositorio)
    {
        Repositorio = repositorio;
    }
}