using FeatureFlag.Domain;
using FeatureFlag.Dominio.Dtos;
using FeatureFlag.Dominio.Infra;

namespace FeatureFlag.Dominio;

public interface IServConsumidor : IServBase<Consumidor, IRepConsumidor>
{
}