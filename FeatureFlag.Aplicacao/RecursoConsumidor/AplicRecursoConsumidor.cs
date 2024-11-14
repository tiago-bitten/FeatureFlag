using FeatureFlag.Aplicacao.Infra;
using FeatureFlag.Domain;
using FeatureFlag.Dominio.RecursoConsumidor.Dtos;
using Microsoft.EntityFrameworkCore;

namespace FeatureFlag.Aplicacao;

public class AplicRecursoConsumidor : AplicBase, IAplicRecursoConsumidor
{
    #region Ctor
    private readonly IServRecursoConsumidor _servRecursoConsumidor;
    private readonly IServConsumidor _servConsumidor;

    public AplicRecursoConsumidor(IServRecursoConsumidor servRecursoConsumidor,
                                  IServConsumidor servConsumidor)
    {
        _servRecursoConsumidor = servRecursoConsumidor;
        _servConsumidor = servConsumidor;
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
            
        throw new NotImplementedException("Lógica de criar consumidor ainda não foi implementada.");

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