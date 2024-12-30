using FeatureFlag.Domain;
using FeatureFlag.Dominio.Infra;

namespace FeatureFlag.Dominio;

public class ServControleAcessoConsumidor : ServBase<ControleAcessoConsumidor, IRepControleAcessoConsumidor>, IServControleAcessoConsumidor
{
    #region Ctor
    public ServControleAcessoConsumidor(IRepControleAcessoConsumidor repositorio) : base(repositorio)
    {
    }
    #endregion
    
    #region RemoverPorRecursoAsync
    public async Task RemoverPorRecursoAsync(Recurso recurso)
    {
        var controleAcessoConsumidores = await Repositorio.RecuperarPorRecursoAsync(recurso.Id);
        foreach (var controleAcessoConsumidor in controleAcessoConsumidores)
        {
            await RemoverAsync(controleAcessoConsumidor);
        }
    }
    #endregion
}