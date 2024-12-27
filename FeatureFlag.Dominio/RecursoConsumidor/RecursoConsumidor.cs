using FeatureFlag.Domain;
using FeatureFlag.Dominio.Infra;
using MongoDB.Bson;

namespace FeatureFlag.Dominio;

public sealed class RecursoConsumidor : EntidadeBase
{
    public RecursoEmbedded Recurso { get; set; } = new();
    public ConsumidorEmbedded Consumidor { get; set; } = new();
    public EnumStatusRecursoConsumidor Status { get; set; }
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
        Recurso = new RecursoEmbedded
        {
            Id = recurso.Id,
            Identificador = recurso.Identificador
        };
        
        Consumidor = new ConsumidorEmbedded
        {
            Id = consumidor.Id,
            Identificador = consumidor.Identificador
        };
    }
    #endregion
    
    #region Embeddeds

    #region ConsumidorEmbedded
    public class ConsumidorEmbedded
    {
        public ObjectId Id { get; set; }
        public string Identificador { get; set; }
    }
    #endregion

    #region RecursoEmbedded
    public class RecursoEmbedded
    {
        public ObjectId Id { get; set; }
        public string Identificador { get; set; }
    }
    #endregion
    #endregion
}

public enum EnumStatusRecursoConsumidor
{
    Habilitado = 1,
    Desabilitado = 2
}