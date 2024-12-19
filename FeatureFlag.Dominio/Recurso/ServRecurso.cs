using FeatureFlag.Dominio.Infra;

namespace FeatureFlag.Domain;

public class ServRecurso : ServBase<Recurso, IRepRecurso>, IServRecurso
{
    #region Ctor
    public ServRecurso(IRepRecurso repositorio) : base(repositorio)
    {
    }
    #endregion
}