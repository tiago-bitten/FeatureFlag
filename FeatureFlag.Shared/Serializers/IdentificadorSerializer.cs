using FeatureFlag.Shared.ValueObjects;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace FeatureFlag.Shared.Serializers;

public class IdentificadorSerializer : SerializerBase<Identificador>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Identificador value)
    {
        context.Writer.WriteString(value.Valor);
    }

    public override Identificador Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var valor = context.Reader.ReadString();
        return new Identificador(valor);
    }
}