using FeatureFlag.Domain;
using FeatureFlag.Dominio.Infra;

namespace FeatureFlag.Dominio;

public interface IServControleAcessoConsumidor : IServBase<ControleAcessoConsumidor, IRepControleAcessoConsumidor>
{
    Task RemoverPorRecursoAsync(Recurso recurso);
    Task RemoverPorConsumidorAsync(Consumidor consumidor);
}