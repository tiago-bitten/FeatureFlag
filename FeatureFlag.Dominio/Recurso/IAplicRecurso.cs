using FeatureFlag.Domain.Dtos;
using FeatureFlag.Dominio.Infra;

namespace FeatureFlag.Domain;

public interface IAplicRecurso
{
    Task<RecursoResponse> AdicionarAsync(AdicionarRecursoRequest request);
    Task<RecursoResponse> AlterarAsync(string identificador, AlterarRecursoRequest request);
    Task<List<RecursoResponse>> RecuperarTodosAsync();
    Task SinconizarEmbeddedsAsync(SincronizarEmbeddeds<Recurso> recursoAtualizado);
    Task<RecursoResponse> RecuperarPorIdentificadorAsync(string identificador);
    Task RemoverAsync(string identificador);
}