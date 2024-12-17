using MongoDB.Driver;
using FeatureFlag.Dominio.Infra;

namespace FeatureFlag.Repositorio.Infra;

public class RepBase<T> : IRepBase<T> where T : EntidadeBase
{
    #region Ctor
    protected readonly IMongoCollection<T> Collection;

    public RepBase(IMongoCollection<T> collection)
    {
        Collection = collection;
    }

    #endregion

    #region Adicionar
    public async Task AdicionarAsync(T entidade)
    {
        await Collection.InsertOneAsync(entidade);
    }
    #endregion

    #region RecuperarPorId
    public async Task<T?> RecuperarPorIdAsync(string id)
    {
        var filter = Builders<T>.Filter.Eq(e => e.Id, id);
        return await Collection.Find(filter).FirstOrDefaultAsync();
    }
    #endregion

    #region RecuperarTodos
    public async Task<List<T>> RecuperarTodosAsync()
    {
        return await Collection.Find(Builders<T>.Filter.Empty).ToListAsync();
    }
    #endregion

    #region Atualizar
    public async Task AtualizarAsync(T entidade)
    {
        var filter = Builders<T>.Filter.Eq(e => e.Id, entidade.Id);
        await Collection.ReplaceOneAsync(filter, entidade);
    }
    #endregion

    #region Remover
    public async Task RemoverAsync(string id)
    {
        var filter = Builders<T>.Filter.Eq(e => e.Id, id);
        await Collection.DeleteOneAsync(filter);
    }
    #endregion

    #region Contar
    public async Task<int> CountAsync()
    {
        var count = await Collection.CountDocumentsAsync(Builders<T>.Filter.Empty);
        return (int)count;
    }
    #endregion
}