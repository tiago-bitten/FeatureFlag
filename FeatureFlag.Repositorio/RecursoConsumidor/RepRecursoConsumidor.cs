using FeatureFlag.Repositorio.Infra;
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
    public async Task<RecursoConsumidor?> RecuperarPorRecursoEConsumidorAsync(string codigoRecurso, string codigoConsumidor)
    {
        var filter = Builders<RecursoConsumidor>.Filter.And(
            Builders<RecursoConsumidor>.Filter.Eq(x => x.Recurso.Id, codigoRecurso),
            Builders<RecursoConsumidor>.Filter.Eq(x => x.Consumidor.Id, codigoConsumidor)
        );

        return await Collection.Find(filter).FirstOrDefaultAsync();
    }
    #endregion

    #region RecuperarPorConsumidorAsync
    public async Task<List<RecursoConsumidor>> RecuperarPorConsumidorAsync(string codigoConsumidor)
    {
        var filter = Builders<RecursoConsumidor>.Filter.Eq(x => x.Consumidor.Id, codigoConsumidor);
        return await Collection.Find(filter).ToListAsync();
    }
    #endregion

    #region RecuperarPorRecursoAsync
    public async Task<List<RecursoConsumidor>> RecuperarPorRecursoAsync(string codigoRecurso)
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