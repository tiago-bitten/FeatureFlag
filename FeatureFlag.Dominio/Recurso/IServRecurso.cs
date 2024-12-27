using FeatureFlag.Domain.Dtos;
using FeatureFlag.Dominio.Infra;

namespace FeatureFlag.Domain;

public interface IServRecurso : IServBase<Recurso, IRepRecurso>
{
    Task<decimal> CalcularPorcentagemAsync(Recurso recurso, int? totalConsumidores = null);
}