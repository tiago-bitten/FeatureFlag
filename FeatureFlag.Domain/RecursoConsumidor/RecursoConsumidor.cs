using FeatureFlag.Domain.Infra;

namespace FeatureFlag.Domain;

public sealed class RecursoConsumidor : EntidadeBase
{
    public Guid CodigoRecurso { get; set; }
    public Guid CodigoConsumidor { get; set; }
    public EnumStatusRecursoConsumidor Status { get; private set; }
    
    //
    
    public Recurso Recurso { get; set; }
    public Consumidor Consumidor { get; set; }
    
    #region Status
    public void Habilitar() => Status = EnumStatusRecursoConsumidor.Habilitado;
    public void Desabilitar() => Status = EnumStatusRecursoConsumidor.Desabilitado;
    #endregion
}

public enum EnumStatusRecursoConsumidor
{
    Habilitado = 1,
    Desabilitado = 2
}