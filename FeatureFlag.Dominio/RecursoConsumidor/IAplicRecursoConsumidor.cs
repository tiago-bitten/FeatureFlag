using FeatureFlag.Domain.Dtos;
using FeatureFlag.Dominio.Dtos;

namespace FeatureFlag.Dominio;

public interface IAplicRecursoConsumidor
{
    Task<RecursoConsumidorResponse> RecuperarPorRecursoConsumidorAsync(RecuperarPorRecursoConsumidorParam param);
}