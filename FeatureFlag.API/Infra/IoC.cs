using System.Text.Json;
using FeatureFlag.Aplicacao;
using FeatureFlag.Domain;
using FeatureFlag.Dominio;
using FeatureFlag.Repositorio;
using FeatureFlag.Repositorio.Infra;
using MongoDB.Driver;

namespace FeatureFlag.API.Infra;

public static class IoC
{
    #region Banco de dados
    public static IServiceCollection ConfigurarBanco(this IServiceCollection services, string connectionString, string databaseName)
    {
        services.AddSingleton<IMongoClient>(_ => new MongoClient(connectionString));
        services.AddSingleton<IMongoDatabase>(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            var database = client.GetDatabase(databaseName);
            
            DocumentsRegistration.Inicializar(database, recriarBanco: true);
            
            return database;
        });

        services.AddScoped<MongoDbContext>();
        return services;
    }

    #endregion

    #region AutoMapper
    public static IServiceCollection ConfigurarAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(RecursoProfile).Assembly);
        services.AddAutoMapper(typeof(ConsumidorProfile).Assembly);
        services.AddAutoMapper(typeof(RecursoConsumidorProfile).Assembly);
        services.AddAutoMapper(typeof(ControleAcessoConsumidorProfile).Assembly);
        
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
        services.AddScoped<IServRecurso, ServRecurso>();
        services.AddScoped<IServConsumidor, ServConsumidor>();
        services.AddScoped<IServRecursoConsumidor, ServRecursoConsumidor>();
        services.AddScoped<IServControleAcessoConsumidor, ServControleAcessoConsumidor>();
        
        return services;
    }
    #endregion

    #region Serviços de aplicação
    public static IServiceCollection ConfigurarServicosAplicacao(this IServiceCollection services)
    {
        services.AddScoped<IAplicRecurso, AplicRecurso>();
        services.AddScoped<IAplicConsumidor, AplicConsumidor>();
        services.AddScoped<IAplicRecursoConsumidor, AplicRecursoConsumidor>();
        services.AddScoped<IAplicControleAcessoConsumidor, AplicControleAcessoConsumidor>();
        
        return services;
    }
    #endregion
    
    public static IServiceCollection ConfigurarJsonOptions(this IServiceCollection services)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = null,
        };

        services.AddSingleton(options);
        return services;
    }

}