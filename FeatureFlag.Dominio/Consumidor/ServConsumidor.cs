using FeatureFlag.Domain;
using FeatureFlag.Domain.Infra;

namespace FeatureFlag.Dominio;

public class ServConsumidor : ServBase<Consumidor, IRepConsumidor>
{
    #region Ctor
    public ServConsumidor(IRepConsumidor repositorio) : base(repositorio)
    {
    }
    #endregion
}