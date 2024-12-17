using FeatureFlag.Dominio.Infra;

namespace FeatureFlag.Domain;

public interface IRepRecurso : IRepBase<Recurso>
{
    Task<decimal> RecuperarPorcentagemPorIdentificadorAsync(string identificador);
    Task<Recurso> RecuperarPorIdentificadorAsync(string identificador);
}