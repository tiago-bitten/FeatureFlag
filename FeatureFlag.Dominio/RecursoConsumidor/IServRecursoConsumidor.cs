using FeatureFlag.Dominio;
using FeatureFlag.Dominio.Dtos;
using FeatureFlag.Dominio.Infra;

namespace FeatureFlag.Domain;

public interface IServRecursoConsumidor : IServBase<RecursoConsumidor, IRepRecursoConsumidor>
{
    Task<bool> RecuperarDisponibilidadeAsync(RecursoConsumidor recursoConsumidor);
    Task AtualizarStatusAsync(RecursoConsumidor recursoConsumidor);
    Task<RecursoConsumidorResponse> RetornarCemPorcentoAtivoAsync(RecuperarPorRecursoConsumidorParam param);
    Task<RecursoConsumidorResponse> RetornarZeroPorcentoAtivoAsync(RecuperarPorRecursoConsumidorParam param);
}