using FeatureFlag.Domain;
using FeatureFlag.Dominio.Infra;

namespace FeatureFlag.Dominio;

public sealed class RecursoConsumidor : EntidadeBase
{
    public RecursoEmbedded Recurso { get; private set; }
    public ConsumidorEmbedded Consumidor { get; private set; }
    public EnumStatusRecursoConsumidor Status { get; private set; }

    #region Setters
    public void Habilitar() => Status = EnumStatusRecursoConsumidor.Habilitado;
    public void Desabilitar() => Status = EnumStatusRecursoConsumidor.Desabilitado;
    #endregion

    #region Fábrica estática
    private RecursoConsumidor(string codigoRecurso, string codigoConsumidor)
    {
        Consumidor.Id = codigoConsumidor;
        Recurso.Id = codigoRecurso;
    }

    public static RecursoConsumidor Criar(string codigoRecurso, string codigoConsumidor)
    {
        var recursoConsumidor = new RecursoConsumidor(codigoRecurso, codigoConsumidor);

        return recursoConsumidor;
    }    
    #endregion
    
    #region Embeddeds

    #region ConsumidorEmbedded
    public class ConsumidorEmbedded
    {
        public string Id { get; set; }
        public string Identificador { get; set; }
    }
    #endregion

    #region RecursoEmbedded
    public class RecursoEmbedded
    {
        public string Id { get; set; }
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