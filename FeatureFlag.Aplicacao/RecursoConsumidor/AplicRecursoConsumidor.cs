using FeatureFlag.Aplicacao.Dtos;
using FeatureFlag.Aplicacao.Infra;
using FeatureFlag.Domain;

namespace FeatureFlag.Aplicacao;

public class AplicRecursoConsumidor : AplicBase, IAplicRecursoConsumidor
{
    #region Ctor
    private readonly IServRecursoConsumidor _servRecursoConsumidor;

    public AplicRecursoConsumidor(IServRecursoConsumidor servRecursoConsumidor)
    {
        _servRecursoConsumidor = servRecursoConsumidor;
    }
    #endregion

    public async Task<RecursoConsumidorResponse> RecuperarPorRecursoConsumidorAsync(string descricaoRecurso, string identificadorRecurso)
    {
        
    }
}