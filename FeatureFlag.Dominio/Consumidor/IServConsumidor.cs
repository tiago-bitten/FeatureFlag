using FeatureFlag.Dominio.Infra;

namespace FeatureFlag.Dominio;

public interface IServConsumidor : IServBase<Consumidor, IRepConsumidor>
{
    Task<Consumidor> RecuperarPorIdentificadorAsync(string identificador);
}