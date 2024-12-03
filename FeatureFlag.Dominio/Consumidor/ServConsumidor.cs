using FeatureFlag.Dominio.Infra;

namespace FeatureFlag.Dominio;

public class ServConsumidor : ServBase<Consumidor, IRepConsumidor>, IServConsumidor
{
    #region Ctor
    public ServConsumidor(IRepConsumidor repositorio) : base(repositorio)
    {
    }
    #endregion
}