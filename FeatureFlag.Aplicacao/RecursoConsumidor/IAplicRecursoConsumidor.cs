using FeatureFlag.Aplicacao.Dtos;

namespace FeatureFlag.Aplicacao;

public interface IAplicRecursoConsumidor
{
    Task<RecursoConsumidorResponse> RecuperarPorRecursoConsumidorAsync(string descricaoRecurso, string identificadorRecurso);
}