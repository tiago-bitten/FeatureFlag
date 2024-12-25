using FeatureFlag.Domain.Dtos;
using FeatureFlag.Dominio.Infra;

namespace FeatureFlag.Domain;

public interface IServRecurso : IServBase<Recurso, IRepRecurso>
{
    Task<Recurso> AlterarPorcentagemAsync(Recurso recurso, decimal novaPorcentagem);   
}