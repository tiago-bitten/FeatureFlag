using FeatureFlag.Domain.Infra;

namespace FeatureFlag.Domain;

public class ServRecurso : ServBase<Recurso, IRepRecurso>
{
    #region Ctor
    public ServRecurso(IRepRecurso repositorio) : base(repositorio)
    {
    }
    #endregion
}