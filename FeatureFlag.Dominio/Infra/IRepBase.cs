namespace FeatureFlag.Dominio.Infra;

public interface IRepBase<T> where T : EntidadeBase
{
    Task AdicionarAsync(T entidade);
    Task<T?> RecuperarPorIdAsync(string id);
    Task<List<T>> RecuperarTodosAsync();
    Task AtualizarAsync(T entidade);
    Task RemoverAsync(string id);
    Task<int> CountAsync();
}