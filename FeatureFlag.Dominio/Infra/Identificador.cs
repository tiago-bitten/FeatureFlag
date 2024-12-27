using MongoDB.Bson;

namespace FeatureFlag.Dominio.Infra;

public abstract class IdentificadorObjectId
{
    public ObjectId Id { get; set; }
}