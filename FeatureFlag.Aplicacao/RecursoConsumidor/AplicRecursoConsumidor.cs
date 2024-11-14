using FeatureFlag.Aplicacao.Infra;
using FeatureFlag.Domain;
using FeatureFlag.Dominio;
using FeatureFlag.Dominio.RecursoConsumidor;
using FeatureFlag.Dominio.RecursoConsumidor.Dtos;
using Microsoft.EntityFrameworkCore;

namespace FeatureFlag.Aplicacao;

public class AplicRecursoConsumidor : AplicBase, IAplicRecursoConsumidor
{
    #region Ctor
    private readonly IServRecursoConsumidor _servRecursoConsumidor;
    private readonly IServConsumidor _servConsumidor;
    private readonly IServRecurso _servRecurso;

    public AplicRecursoConsumidor(IServRecursoConsumidor servRecursoConsumidor,
                                  IServConsumidor servConsumidor,
                                  IServRecurso servRecurso)
    {
        _servRecursoConsumidor = servRecursoConsumidor;
        _servConsumidor = servConsumidor;
        _servRecurso = servRecurso;
    }
    #endregion

    #region RecuperarPorRecursoConsumidorAsync
    public async Task<RecursoConsumidorResponse> RecuperarPorRecursoConsumidorAsync(RecuperarPorRecursoConsumidorParam param)
    {
        var recursoConsumidor = await _servRecursoConsumidor.Repositorio
            .RecuperarPorRecursoConsumidorAsync(param.DescricaoRecurso, param.IdentificadorRecurso);

        if (recursoConsumidor is not null)
            return recursoConsumidor;
        
        var novoConsumidor = Consumidor.Criar(param.IdentificadorRecurso);

        await IniciarTransacaoAsync();
        await _servConsumidor.AdicionarAsync(novoConsumidor);
        await PersistirTransacaoAsync();
        
        return RecursoConsumidorResponse.ConsumidorSemRecurso(novoConsumidor, param.DescricaoRecurso);
    }
    #endregion
    
    #region RecuperarPorConsumidorAsync
    public async Task<List<RecursoConsumidorResponse>> RecuperarPorConsumidorAsync(RecuperarPorConsumidorParam param)
    {
        var recursosConsumidor = await _servRecursoConsumidor.Repositorio
            .RecuperarPorConsumidor(param.IdentificadorConsumidor)
            .ToListAsync();

        return recursosConsumidor;
    }

    #endregion
}