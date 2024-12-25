using FeatureFlag.Domain.Dtos;

namespace FeatureFlag.Domain;

public interface IAplicRecurso
{
    Task<RecursoResponse> AdicionarAsync(AdicionarRecursoRequest request);
    Task<RecursoResponse> AlterarAsync(AlterarRecursoRequest request);
    Task<RecursoResponse> AlterarPorcentagemAsync(AlterarRecursoPorcentagemRequest request);
}