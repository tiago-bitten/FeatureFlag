namespace FeatureFlag.Dominio.Infra;

public interface IServBase<TEntidade, TRepositorio>
    where TEntidade : EntidadeBase
    where TRepositorio : IRepBase<TEntidade>
{
    TRepositorio Repositorio { get; }
    
    Task AdicionarAsync(TEntidade entidade);
    Task AtualizarAsync(TEntidade entidade);
    Task AtualizarVariosAsync(List<TEntidade> entidades);
    Task RemoverAsync(TEntidade entidade);
}