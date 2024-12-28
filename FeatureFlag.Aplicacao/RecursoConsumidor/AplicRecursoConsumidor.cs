using AutoMapper;
using FeatureFlag.Aplicacao.Infra;
using FeatureFlag.Domain;
using FeatureFlag.Dominio;
using FeatureFlag.Dominio.Dtos;
using FeatureFlag.Shared.Extensions;

namespace FeatureFlag.Aplicacao;

public class AplicRecursoConsumidor : AplicBase, IAplicRecursoConsumidor
{
    #region Ctor
    private readonly IServRecursoConsumidor _servRecursoConsumidor;
    private readonly IServConsumidor _servConsumidor;
    private readonly IServRecurso _servRecurso;

    public AplicRecursoConsumidor(IServRecursoConsumidor servRecursoConsumidor,
                                  IServConsumidor servConsumidor,
                                  IServRecurso servRecurso,
                                  IMapper mapper)
        : base(mapper)
    {
        _servRecursoConsumidor = servRecursoConsumidor;
        _servConsumidor = servConsumidor;
        _servRecurso = servRecurso;
    }

    #endregion

    #region RecuperarPorRecursoConsumidorAsync
    public async Task<RecursoConsumidorResponse> RecuperarPorRecursoConsumidorAsync(RecuperarPorRecursoConsumidorParam param)
    {
        var recurso = await _servRecurso.Repositorio.RecuperarPorIdentificadorAsync(param.IdentificadorRecurso);
        recurso.ThrowIfNull("Recurso não foi encontrado.");

        var consumidor = await _servConsumidor.Repositorio.RecuperarPorIdentificadorAsync(param.IdentificadorConsumidor);
        if (consumidor is null)
        {
            consumidor = new Consumidor(param.IdentificadorConsumidor);
            await _servConsumidor.AdicionarAsync(consumidor);
        }

        switch (recurso.Porcentagem)
        {
            case 100:
                return await _servRecursoConsumidor.RetornarCemPorcentoAtivoAsync(param);
            case 0:
                return await _servRecursoConsumidor.RetornarZeroPorcentoAtivoAsync(param);
        }

        var recursoConsumidor = await _servRecursoConsumidor.Repositorio.RecuperarPorRecursoEConsumidorAsync(param.IdentificadorRecurso, param.IdentificadorConsumidor);
        if (recursoConsumidor is null)
        {
            recursoConsumidor = new RecursoConsumidor(recurso, consumidor);
            await _servRecursoConsumidor.AdicionarAsync(recursoConsumidor);
        }

        if (recursoConsumidor.Congelado)
        {
            return RecursoConsumidorResponse.Ativo(recurso.Identificador, consumidor.Identificador);
        }

        await _servRecursoConsumidor.AtualizarDisponibilidadeAsync(recursoConsumidor, recurso, consumidor);
        await _servRecurso.AtualizarAsync(recurso);
        await _servConsumidor.AtualizarAsync(consumidor);

        var response = Mapper.Map<RecursoConsumidorResponse>(recursoConsumidor);

        return response;
    }

    #endregion
}