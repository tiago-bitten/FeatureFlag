using FeatureFlag.Domain.Dtos;
using FeatureFlag.Dominio.Infra;

namespace FeatureFlag.Domain;

public interface IServRecurso : IServBase<Recurso, IRepRecurso>
{
    Task<Recurso> AlterarPorcentagemAsync(Recurso recurso, decimal novaPorcentagem);   
    Task<decimal> CalcularPorcentagemAsync(Recurso recurso, int? totalConsumidores = null);
    Task VerificarPorcentagemAlvoAtingidaAsync(Recurso recurso, int totalConsumidores);
}