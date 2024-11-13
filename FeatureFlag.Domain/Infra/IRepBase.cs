namespace FeatureFlag.Domain.Infra;

public interface IRepBase<T> where T : EntidadeBase
{
    Task<T> AdicionarAsync(T entidade);
    Task<T> RecuperarPorIdAsync(int id, params string[]? includes);
    IQueryable<T> RecuperarTodos(params string[]? includes);
    void Atualizar(T entidade);
    void Remover(int id);
}