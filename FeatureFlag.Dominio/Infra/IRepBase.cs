namespace FeatureFlag.Dominio.Infra;

public interface IRepBase<T> where T : EntidadeBase
{
    Task AdicionarAsync(T entidade);
    Task<T?> RecuperarPorIdAsync(Guid id, params string[]? includes);
    IQueryable<T> RecuperarTodos(params string[]? includes);
    void Atualizar(T entidade);
    void Remover(Guid id);
    Task<int> CountAsync();
}