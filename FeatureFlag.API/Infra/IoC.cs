using FeatureFlag.Domain;
using FeatureFlag.Dominio;
using FeatureFlag.Repositorio;
using MongoDB.Driver;

namespace FeatureFlag.API.Infra;

public static class IoC
{
    #region Banco de dados
    public static IServiceCollection ConfigurarBanco(this IServiceCollection services, string connectionString, string databaseName)
    {
        services.AddSingleton<IMongoClient>(sp => new MongoClient(connectionString));
        services.AddSingleton<IMongoDatabase>(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(databaseName);
        });
        
        return services;
    }
    #endregion
    
    #region Repositorios
    public static IServiceCollection ConfigurarRepositorios(this IServiceCollection services)
    {
        services.AddScoped<IRepRecurso, RepRecurso>();
        services.AddScoped<IRepConsumidor, RepConsumidor>();
        services.AddScoped<IRepRecursoConsumidor, RepRecursoConsumidor>();
        services.AddScoped<IRepControleAcessoConsumidor, RepControleAcessoConsumidor>();
        
        return services;
    }
    #endregion
    
    #region Serviços
    public static IServiceCollection ConfigurarServicos(this IServiceCollection services)
    {
        return services;
    }
    #endregion
}