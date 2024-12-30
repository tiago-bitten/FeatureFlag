using FeatureFlag.Domain.Dtos;

namespace FeatureFlag.Domain;

public interface IAplicRecurso
{
    Task<RecursoResponse> AdicionarAsync(AdicionarRecursoRequest request);
    Task<RecursoResponse> AlterarAsync(string identificador, AlterarRecursoRequest request);
    Task<List<RecursoResponse>> RecuperarTodosAsync();
    Task SinconizarRecursosEmbeddedAsync(SincronizarRecursoRequest request);
    Task<RecursoResponse> RecuperarPorIdentificadorAsync(string identificador);
}