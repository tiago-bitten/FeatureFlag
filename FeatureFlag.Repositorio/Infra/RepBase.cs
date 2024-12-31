using MongoDB.Driver;
using FeatureFlag.Dominio.Infra;
using FeatureFlag.Shared.Extensions;
using MongoDB.Bson;

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
    public async Task<T?> RecuperarPorIdAsync(ObjectId id)
    {
        var filter = Builders<T>.Filter.Eq(e => e.Id, id).FilterBase();
        return await Collection.Find(filter).FirstOrDefaultAsync();
    }
    #endregion

    #region RecuperarTodos
    public async Task<List<T>> RecuperarTodosAsync()
    {
        var filter = Builders<T>.Filter.Empty.FilterBase();
        return await Collection.Find(filter).ToListAsync();
    }
    #endregion

    #region Atualizar
    public async Task AtualizarAsync(T entidade)
    {
        var filter = Builders<T>.Filter.Eq(e => e.Id, entidade.Id).FilterBase();
        await Collection.ReplaceOneAsync(filter, entidade);
    }
    #endregion
    
    #region AtualizarVarios
    public async Task AtualizarVariosAsync(List<T> entidades)
    {
        var updates = entidades.Select(e =>
            new ReplaceOneModel<T>(
                Builders<T>.Filter.Eq(x => x.Id, e.Id).FilterBase(),
                e
            )
        ).ToList();
        await Collection.BulkWriteAsync(updates);
    }
    #endregion

    #region Remover
    public async Task RemoverAsync(ObjectId id)
    {
        var filter = Builders<T>.Filter.Eq(e => e.Id, id).FilterBase();
        await Collection.DeleteOneAsync(filter);
    }
    #endregion

    #region Contar
    public async Task<int> CountAsync()
    {
        var filter = Builders<T>.Filter.Empty.FilterBase();
        var count = await Collection.CountDocumentsAsync(filter);
        return (int)count;
    }
    #endregion
}