using FeatureFlag.Domain;
using FeatureFlag.Dominio.Dtos;
using FeatureFlag.Dominio.Infra;

namespace FeatureFlag.Dominio;

public interface IServConsumidor : IServBase<Consumidor, IRepConsumidor>
{
    void SincronizarRecursoConsumidores(Consumidor consumidorAtualizado, List<RecursoConsumidor> recursosConsumidores);
    void SincronizarControleAcessoConsumidores(Consumidor consumidorAtualizado, List<ControleAcessoConsumidor> controleAcessoConsumidores);
}