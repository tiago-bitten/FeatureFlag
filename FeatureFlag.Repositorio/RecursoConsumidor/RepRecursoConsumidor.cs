using FeatureFlag.Repositorio.Infra;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FeatureFlag.Dominio;

public class RepRecursoConsumidor : RepBase<RecursoConsumidor>, IRepRecursoConsumidor
{
    #region Ctor
    public RepRecursoConsumidor(MongoDbContext context) 
        : base(context.RecursoConsumidores)
    {
    }
    #endregion

    #region RecuperarPorRecursoEConsumidorAsync
    public async Task<RecursoConsumidor?> RecuperarPorRecursoEConsumidorAsync(string identificadorRecurso, string identificadorConsumidor)
    {
        var filter = Builders<RecursoConsumidor>.Filter.And(
            Builders<RecursoConsumidor>.Filter.Eq(x => x.Recurso.Identificador, identificadorRecurso),
            Builders<RecursoConsumidor>.Filter.Eq(x => x.Consumidor.Identificador, identificadorConsumidor)
        );

        return await Collection.Find(filter).FirstOrDefaultAsync();
    }
    #endregion

    #region RecuperarPorConsumidorAsync
    public async Task<List<RecursoConsumidor>> RecuperarPorConsumidorAsync(ObjectId codigoConsumidor)
    {
        var filter = Builders<RecursoConsumidor>.Filter.Eq(x => x.Consumidor.Id, codigoConsumidor);
        return await Collection.Find(filter).ToListAsync();
    }
    #endregion

    #region RecuperarPorRecursoAsync
    public async Task<List<RecursoConsumidor>> RecuperarPorRecursoAsync(ObjectId codigoRecurso)
    {
        var filter = Builders<RecursoConsumidor>.Filter.Eq(x => x.Recurso.Id, codigoRecurso);
        return await Collection.Find(filter).ToListAsync();
    }
    #endregion

    #region RecuperarPorStatusAsync
    public async Task<List<RecursoConsumidor>> RecuperarPorStatusAsync(EnumStatusRecursoConsumidor status)
    {
        var filter = Builders<RecursoConsumidor>.Filter.Eq(x => x.Status, status);
        return await Collection.Find(filter).ToListAsync();
    }
    #endregion
}