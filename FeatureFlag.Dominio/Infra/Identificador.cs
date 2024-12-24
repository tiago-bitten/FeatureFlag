using MongoDB.Bson;

namespace FeatureFlag.Dominio.Infra;

public abstract class Identificador
{
    public ObjectId Id { get; set; }
}