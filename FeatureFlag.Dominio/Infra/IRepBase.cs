using MongoDB.Bson;

namespace FeatureFlag.Dominio.Infra;

public interface IRepBase<T> where T : EntidadeBase
{
    Task AdicionarAsync(T entidade);
    Task<T?> RecuperarPorIdAsync(ObjectId id);
    Task<List<T>> RecuperarTodosAsync();
    Task AtualizarAsync(T entidade);
    Task AtualizarVariosAsync(List<T> entidades);
    Task RemoverAsync(ObjectId id);
    Task<int> CountAsync();
}