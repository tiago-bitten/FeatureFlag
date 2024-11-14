using FeatureFlag.Domain.Dtos;

namespace FeatureFlag.Domain;

public interface IAplicRecurso
{
    Task<RecursoResponse> AdicionarAsync(CriarRecursoRequest request);
}