using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace FeatureFlag.Repositorio.Infra;

public static class DocumentsRegistration
{
    private static IMongoDatabase? _database;
    private static bool _recreateDatabase;

    public static void Inicializar(IMongoDatabase? database, bool recriarBanco = false)
    {
        _database = database ?? throw Error();
        _recreateDatabase = recriarBanco;
        Registrar();
    }

    private static void Registrar()
    {
        if (_recreateDatabase)
        {
            RecriarColecoes();
        }

        RegistrarConfigurador(new ConsumidorConfig());
        RegistrarConfigurador(new RecursoConfig());
        RegistrarConfigurador(new RecursoConsumidorConfig());
        RegistrarConfigurador(new ControleAcessoConsumidorConfig());
    }

    private static void RecriarColecoes()
    {
        var colecoes = _database.ListCollectionNames().ToList();
        foreach (var colecao in colecoes)
        {
            _database.DropCollection(colecao);
            Console.WriteLine($"Coleção {colecao} foi excluída.");
        }
    }

    private static void RegistrarConfigurador<T>(IDocumentConfiguration<T> configurador) where T : class
    {
        if (BsonClassMap.IsClassMapRegistered(typeof(T)))
        {
            return;
        }

        var bsonClassMap = new BsonClassMap<T>(configurador.Configurar);
        BsonClassMap.RegisterClassMap(bsonClassMap);

        CriarColecaoSeNaoExistir(typeof(T).Name);
    }

    private static void CriarColecaoSeNaoExistir(string nomeColecao)
    {
        var colecoesExistentes = _database.ListCollectionNames().ToList();
        if (colecoesExistentes.Contains(nomeColecao))
        {
            return;
        }
        
        _database.CreateCollection(nomeColecao);
        Console.WriteLine($"Coleção {nomeColecao} foi criada.");
    }
    
    private static Exception Error()
    {
        throw new ArgumentNullException(nameof(_database));
    }
}