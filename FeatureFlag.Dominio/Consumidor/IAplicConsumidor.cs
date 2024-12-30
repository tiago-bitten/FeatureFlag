using FeatureFlag.Domain.Dtos;
using FeatureFlag.Dominio.Dtos;
using FeatureFlag.Dominio.Infra;

namespace FeatureFlag.Dominio;

public interface IAplicConsumidor
{
    Task<ConsumidorResponse> AdicionarAsync(AdicionarConsumidorRequest request);
    Task<ConsumidorResponse> AlterarAsync(string identificador, AlterarConsumidorRequest request);
    Task<ConsumidorResponse> RecuperarPorIdentificadorAsync(string identificador);
    Task SincronizarEmbeddedsAsync(SincronizarEmbeddeds<Consumidor> consumidorAtualizado);
}