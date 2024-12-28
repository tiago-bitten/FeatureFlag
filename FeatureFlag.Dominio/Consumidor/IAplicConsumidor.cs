using FeatureFlag.Dominio.Dtos;

namespace FeatureFlag.Dominio;

public interface IAplicConsumidor
{
    Task<ConsumidorResponse> AdicionarAsync(AdicionarConsumidorRequest request);
    Task<ConsumidorResponse> AlterarAsync(string identificador, AlterarConsumidorRequest request);
    Task<ConsumidorResponse> RecuperarPorIdentificadorAsync(string identificador);
}