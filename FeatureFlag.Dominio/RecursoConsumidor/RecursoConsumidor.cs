using FeatureFlag.Domain;
using FeatureFlag.Dominio.Infra;
using MongoDB.Bson;

namespace FeatureFlag.Dominio;

public sealed class RecursoConsumidor : EntidadeBase
{
    public RecursoEmbedded Recurso { get; private set; }
    public ConsumidorEmbedded Consumidor { get; private set; }
    public EnumStatusRecursoConsumidor Status { get; private set; }
    public bool Congelado { get; private set; }

    #region Setters
    public void Habilitar() => Status = EnumStatusRecursoConsumidor.Habilitado;
    public void Desabilitar() => Status = EnumStatusRecursoConsumidor.Desabilitado;
    public void Congelar() => Congelado = true;
    public void Descongelar() => Congelado = false;
    #endregion

    #region Ctor
    public RecursoConsumidor(Recurso recurso, Consumidor consumidor)
    {
        Recurso = new RecursoEmbedded(recurso.Id, recurso.Identificador);
        Consumidor = new ConsumidorEmbedded(consumidor.Id, consumidor.Identificador);
    }
    #endregion
    
    #region Embeddeds

    #region ConsumidorEmbedded
    public class ConsumidorEmbedded
    {
        public ObjectId Id { get; init; }
        public string Identificador { get; set; }
        
        public ConsumidorEmbedded(ObjectId id, string identificador)
        {
            Id = id;
            Identificador = identificador;
        }
    }
    #endregion

    #region RecursoEmbedded
    public class RecursoEmbedded
    {
        public ObjectId Id { get; init; }
        public string Identificador { get; set; }
        
        public RecursoEmbedded(ObjectId id, string identificador)
        {
            Id = id;
            Identificador = identificador;
        }
    }
    #endregion
    #endregion
}

public enum EnumStatusRecursoConsumidor
{
    Habilitado = 1,
    Desabilitado = 2
}