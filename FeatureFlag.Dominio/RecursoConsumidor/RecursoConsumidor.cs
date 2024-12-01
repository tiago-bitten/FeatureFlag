using FeatureFlag.Domain;
using FeatureFlag.Dominio.Infra;

namespace FeatureFlag.Dominio;

public sealed class RecursoConsumidor : EntidadeBase
{
    public Guid CodigoRecurso { get; private set; }
    public Guid CodigoConsumidor { get; private set; }
    public EnumStatusRecursoConsumidor Status { get; private set; }
    
    //
    
    public Recurso Recurso { get; set; }
    public Consumidor Consumidor { get; set; }
    
    #region Regras
    public void ValidarCodigos()
    {
        if (string.IsNullOrWhiteSpace(CodigoRecurso.ToString()) ||
            string.IsNullOrWhiteSpace(CodigoConsumidor.ToString()))
        {
            throw new Exception();
        }
    }
    #endregion
    
    #region Status
    public void Habilitar() => Status = EnumStatusRecursoConsumidor.Habilitado;
    public void Desabilitar() => Status = EnumStatusRecursoConsumidor.Desabilitado;
    #endregion
    
    #region Fábrica estática
    private RecursoConsumidor(Guid codigoRecurso, Guid codigoConsumidor)
    {
        CodigoConsumidor = codigoConsumidor;
        CodigoRecurso = codigoRecurso;
    }

    public static RecursoConsumidor Criar(Guid codigoRecurso, Guid codigoConsumidor)
    {
        var recursoConsumidor = new RecursoConsumidor(codigoRecurso, codigoConsumidor);
        recursoConsumidor.ValidarCodigos();

        return recursoConsumidor;
    }    
    #endregion
}

public enum EnumStatusRecursoConsumidor
{
    Habilitado = 1,
    Desabilitado = 2
}