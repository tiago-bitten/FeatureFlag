using FeatureFlag.Dominio;
using FeatureFlag.Repositorio.Infra;
using MongoDB.Driver;

namespace FeatureFlag.Repositorio;

public class RepConsumidor : RepBase<Consumidor>, IRepConsumidor
{
    #region Ctor
    public RepConsumidor(MongoDbContext context) 
        : base(context.Consumidores)
    {
    }
    #endregion

    #region RecuperarPorIdentificador
    public async Task<Consumidor?> RecuperarPorIdentificadorAsync(string identificador)
    {
        return await Collection
            .Find(x => x.Identificador == identificador)
            .FirstOrDefaultAsync();
    }
    #endregion
}