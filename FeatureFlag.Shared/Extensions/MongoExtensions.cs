using MongoDB.Driver;

namespace FeatureFlag.Shared.Extensions;

public static class MongoExtensions
{
    public static FilterDefinition<T> FilterBase<T>(this FilterDefinition<T> filter)
    {
        var properties = typeof(T).GetProperties();
        var fieldInativo = properties.FirstOrDefault(x => x.Name == "Inativo");
        
        if (fieldInativo is not null)
        {
            return Builders<T>.Filter.And(filter, Builders<T>.Filter.Eq(fieldInativo.Name, false));
        }
        return filter;
    }
}