using FeatureFlag.Domain;
using MongoDB.Driver;
using FeatureFlag.Dominio;

namespace FeatureFlag.Repositorio.Infra
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        #region Ctor
        public MongoDbContext(IMongoDatabase database)
        {
            _database = database;
        }
        #endregion

        #region Collections
        public IMongoCollection<Recurso> Recursos => _database.GetCollection<Recurso>("Recursos");
        public IMongoCollection<Consumidor> Consumidores => _database.GetCollection<Consumidor>("Consumidores");
        public IMongoCollection<RecursoConsumidor> RecursoConsumidores => _database.GetCollection<RecursoConsumidor>("RecursoConsumidores");
        public IMongoCollection<ControleAcessoConsumidor> ControleAcessoConsumidores => _database.GetCollection<ControleAcessoConsumidor>("ControleAcessoConsumidores");
        #endregion
    }
}