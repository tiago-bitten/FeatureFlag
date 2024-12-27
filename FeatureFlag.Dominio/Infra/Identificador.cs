using MongoDB.Bson;

namespace FeatureFlag.Dominio.Infra;

public abstract class IdentificadorObjectId : ICloneable
{
    public ObjectId Id { get; set; }
    public object Clone() => MemberwiseClone();
}