using FeatureFlag.Domain.Dtos;
using FeatureFlag.Dominio;
using FeatureFlag.Dominio.Infra;

namespace FeatureFlag.Domain;

public interface IServRecurso : IServBase<Recurso, IRepRecurso>
{
    Task<decimal> CalcularPorcentagemAsync(Recurso recurso, int? totalConsumidores = null);
    void SincronizarConsumidores(Recurso recursoAtualizado, List<Consumidor> consumidores);
    void SincronizarRecursoConsumidores(Recurso recursoAtualizado, List<RecursoConsumidor> recursoConsumidores);
    void SincronizarControleAcessoConsumidores(Recurso recursoAtualizado, List<ControleAcessoConsumidor> controleAcessoConsumidores);
}