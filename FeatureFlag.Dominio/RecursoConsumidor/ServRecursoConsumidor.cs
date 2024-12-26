﻿using FeatureFlag.Domain;
using FeatureFlag.Dominio.Dtos;
using FeatureFlag.Dominio.Infra;

namespace FeatureFlag.Dominio;

public class ServRecursoConsumidor : ServBase<RecursoConsumidor, IRepRecursoConsumidor>, IServRecursoConsumidor
{
    #region Ctor
    private readonly IServRecurso _servRecurso;
    private readonly IRepConsumidor _repConsumidor;
    private readonly IRepControleAcessoConsumidor _repControleAcessoConsumidor;

    public ServRecursoConsumidor(IRepRecursoConsumidor repositorio,
                                 IRepConsumidor repConsumidor,
                                 IRepControleAcessoConsumidor repControleAcessoConsumidor,
                                 IServRecurso servRecurso)
        : base(repositorio)
    {
        _repConsumidor = repConsumidor;
        _repControleAcessoConsumidor = repControleAcessoConsumidor;
        _servRecurso = servRecurso;
    }
    #endregion
    
    #region AtualizarStatusAsync
    public async Task AtualizarStatusAsync(RecursoConsumidor recursoConsumidor, Recurso recurso, Consumidor consumidor)
    {
        var totalConsumidores = await _repConsumidor.CountAsync();
        var porcentagemAtual = await _servRecurso.CalcularPorcentagemAsync(recurso, totalConsumidores);

        switch (porcentagemAtual.CompareTo(recurso.Porcentagem.Alvo))
        {
            case < 0:
                HabilitarConsumidor(recursoConsumidor, recurso, consumidor);
                break;
            
            case > 0:
                DesabilitarConsumidor(recursoConsumidor, recurso, consumidor);
                break;
            
            default:
                NormalizarStatus(recursoConsumidor, consumidor);
                break;
        }
        
        await _servRecurso.VerificarPorcentagemAlvoAtingidaAsync(recurso, totalConsumidores);
        await AtualizarAsync(recursoConsumidor);
    }

    #region HabilitarConsumidor
    private void HabilitarConsumidor(RecursoConsumidor recursoConsumidor, Recurso recurso, Consumidor consumidor)
    {
        if (recursoConsumidor.Status is EnumStatusRecursoConsumidor.Habilitado)
        {
            return;
        }
        
        recursoConsumidor.Habilitar();
        recurso.Consumidor.Adicionar();
        consumidor.AdicionarRecursoHabilitado(recurso.Identificador);
    }
    #endregion
    
    #region DesabilitarConsumidor
    private void DesabilitarConsumidor(RecursoConsumidor recursoConsumidor, Recurso recurso, Consumidor consumidor)
    {
        if (recursoConsumidor.Status is EnumStatusRecursoConsumidor.Desabilitado || recurso.Porcentagem.Atingido)
        {
            return;
        }
        
        recursoConsumidor.Desabilitar();
        recurso.Consumidor.Remover();
        consumidor.AdicionarRecursoDesabilitado(recurso.Identificador);
    }
    #endregion
    
    #region NormalizarStatus
    private void NormalizarStatus(RecursoConsumidor recursoConsumidor, Consumidor consumidor)
    {
        if (recursoConsumidor.Status is EnumStatusRecursoConsumidor.Habilitado or EnumStatusRecursoConsumidor.Desabilitado)
        {
            return;
        }
        recursoConsumidor.Desabilitar();
        consumidor.AdicionarRecursoDesabilitado(recursoConsumidor.Recurso.Identificador);
    }
    #endregion
    #endregion
    
    #region Retornar 0% ou 100%
    #region RetornarCemPorcentoAsync
    public async Task<RecursoConsumidorResponse> RetornarCemPorcentoAtivoAsync(RecuperarPorRecursoConsumidorParam param)
    {
        var estaNaBlacklist = await _repControleAcessoConsumidor
            .PossuiPorTipoAsync(param.IdentificadorRecurso, param.IdentificadorConsumidor, EnumTipoControle.Blacklist);
            
        return RetornarPermissao(estaNaBlacklist, EnumTipoControle.Blacklist, param);
    }
    #endregion
        
    #region RetornarZeroPorcentoAsync
    public async Task<RecursoConsumidorResponse> RetornarZeroPorcentoAtivoAsync(RecuperarPorRecursoConsumidorParam param)
    {
        var estaNaWhitelist = await _repControleAcessoConsumidor
            .PossuiPorTipoAsync(param.IdentificadorRecurso, param.IdentificadorConsumidor, EnumTipoControle.Whitelist);
            
        return RetornarPermissao(estaNaWhitelist, EnumTipoControle.Whitelist, param);
    }
    #endregion
        
    #region RetornarPermissao
    private RecursoConsumidorResponse RetornarPermissao(bool estaNaLista, EnumTipoControle tipo,
        RecuperarPorRecursoConsumidorParam param)
    {
        return tipo switch
        {
            EnumTipoControle.Whitelist => estaNaLista
                ? RecursoConsumidorResponse.Ativo(param.IdentificadorRecurso, param.IdentificadorConsumidor)
                : RecursoConsumidorResponse.Desabilitado(param.IdentificadorRecurso, param.IdentificadorConsumidor),

            EnumTipoControle.Blacklist => estaNaLista
                ? RecursoConsumidorResponse.Desabilitado(param.IdentificadorRecurso, param.IdentificadorConsumidor)
                : RecursoConsumidorResponse.Ativo(param.IdentificadorRecurso, param.IdentificadorConsumidor),

            _ => throw new NotImplementedException()
        };
    }
    #endregion
    #endregion
}
