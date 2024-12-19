using MongoDB.Bson.Serialization;
using System.Reflection;

namespace FeatureFlag.Repositorio.Infra;

public static class DocumentsRegistration
{
    private static bool _isRegistered;

    public static void Registrar()
    {
        if (_isRegistered) return;

        var configs = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.BaseType != null && t.BaseType.IsGenericType &&
                        t.BaseType.GetGenericTypeDefinition() == typeof(EntidadeBaseConfig<>));

        foreach (var configType in configs)
        {
            var instance = Activator.CreateInstance(configType);
            var metodoConfigurar = configType.GetMethod("Configurar");
            if (instance == null || metodoConfigurar == null) continue;
                
            var tipoEntidade = configType.BaseType?.GetGenericArguments()[0];
            if (tipoEntidade == null) continue;
                
            var bsonClassMap = Activator.CreateInstance(
                typeof(BsonClassMap<>).MakeGenericType(tipoEntidade)
            );

            metodoConfigurar.Invoke(instance, [bsonClassMap]);
            BsonClassMap.RegisterClassMap((BsonClassMap)bsonClassMap!);
        }

        _isRegistered = true;
    }
}