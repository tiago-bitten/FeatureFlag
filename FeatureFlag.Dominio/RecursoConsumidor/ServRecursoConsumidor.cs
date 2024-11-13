using FeatureFlag.Domain.Infra;

namespace FeatureFlag.Domain;

public class ServRecursoConsumidor : ServBase<RecursoConsumidor, IRepRecursoConsumidor>, IServRecursoConsumidor
{
    #region Ctor
    public ServRecursoConsumidor(IRepRecursoConsumidor repositorio) : base(repositorio)
    {
    }
    #endregion
}