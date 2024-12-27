using FeatureFlag.Dominio.Infra;

namespace FeatureFlag.Dominio;

public interface IRepConsumidor : IRepBase<Consumidor>
{
    Task<Consumidor?> RecuperarPorIdentificadorAsync(string identificador);
    Task<bool> ExistePorIdentificadorAsync(string identificador);
}