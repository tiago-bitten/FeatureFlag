using FeatureFlag.Dominio;
using FeatureFlag.Dominio.Dtos;
using FeatureFlag.Dominio.Infra;

namespace FeatureFlag.Domain;

public interface IServRecursoConsumidor : IServBase<RecursoConsumidor, IRepRecursoConsumidor>
{
    Task AtualizarStatusAsync(RecursoConsumidor recursoConsumidor, Recurso recurso, Consumidor consumidor);
    Task<RecursoConsumidorResponse> RetornarCemPorcentoAtivoAsync(RecuperarPorRecursoConsumidorParam param);
    Task<RecursoConsumidorResponse> RetornarZeroPorcentoAtivoAsync(RecuperarPorRecursoConsumidorParam param);
}