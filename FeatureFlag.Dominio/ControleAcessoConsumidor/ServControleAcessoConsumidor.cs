using FeatureFlag.Domain.Infra;

namespace FeatureFlag.Dominio;

public class ServControleAcessoConsumidor : ServBase<ControleAcessoConsumidor, IRepControleAcessoConsumidor>, IServControleAcessoConsumidor
{
    #region Ctor
    public ServControleAcessoConsumidor(IRepControleAcessoConsumidor repositorio) : base(repositorio)
    {
    }
    #endregion
}