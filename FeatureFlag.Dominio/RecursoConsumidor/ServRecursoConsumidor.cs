using System.Runtime.InteropServices.JavaScript;
using FeatureFlag.Domain;
using FeatureFlag.Dominio.Dtos;
using FeatureFlag.Dominio.Infra;
using FeatureFlag.Shared.Extensions;
using FeatureFlag.Shared.Helpers;

namespace FeatureFlag.Dominio;

public class ServRecursoConsumidor : ServBase<RecursoConsumidor, IRepRecursoConsumidor>, IServRecursoConsumidor
{
    #region Ctor
    private readonly IRepRecurso _repRecurso;
    private readonly IRepConsumidor _repConsumidor;
    private readonly IRepControleAcessoConsumidor _repControleAcessoConsumidor;

    public ServRecursoConsumidor(IRepRecurso repRecurso,
                                 IRepRecursoConsumidor repositorio,
                                 IRepConsumidor repConsumidor,
                                 IRepControleAcessoConsumidor repControleAcessoConsumidor)
        : base(repositorio)
    {
        _repRecurso = repRecurso;
        _repConsumidor = repConsumidor;
        _repControleAcessoConsumidor = repControleAcessoConsumidor;
    }
    #endregion
    
    #region AtualizarStatusAsync
    /**
     * TODO:
     * Adicionar um Lock nos consumidores que estão ativos, afim de evitar que sejam desabilitados e habilitados toda hora.
     * Isso em casos onde a porcentagem não foi alterada, então, deve haver uma tolerância na porcentagem, pois pode passar um pouco da configurada 
     */
    public async Task AtualizarStatusAsync(RecursoConsumidor recursoConsumidor, Recurso recurso)
    {
        var totalConsumidores = await _repConsumidor.CountAsync();
        var porcentagemAtual = PorcentagemHelper.Calcular(recurso.Consumidor.TotalHabilitados, totalConsumidores);

        switch (porcentagemAtual.CompareTo(recurso.Porcentagem.Alvo))
        {
            case < 0:
                HabilitarConsumidor(recursoConsumidor, recurso);
                VerificarSeAtingiu(recurso, totalConsumidores);
                break;
            
            case > 0:
                DesabilitarConsumidor(recursoConsumidor, recurso);
                break;
            
            default:
                NormalizarStatus(recursoConsumidor);
                break;
        }
        
        await AtualizarAsync(recursoConsumidor);
    }

    #region HabilitarConsumidor
    private void HabilitarConsumidor(RecursoConsumidor recursoConsumidor, Recurso recurso)
    {
        if (recursoConsumidor.Status == EnumStatusRecursoConsumidor.Habilitado)
            return;
        
        recursoConsumidor.Habilitar();
        recurso.Consumidor.Adicionar(recursoConsumidor.Consumidor.Identificador);
        
        
    }
    #endregion
    
    #region DesabilitarConsumidor
    private void DesabilitarConsumidor(RecursoConsumidor recursoConsumidor, Recurso recurso)
    {
        switch (recursoConsumidor.Status)
        {
            case EnumStatusRecursoConsumidor.Desabilitado:
                return;
            
            case EnumStatusRecursoConsumidor.Habilitado:
                if (!recurso.Porcentagem.Atingido)
                {
                    recurso.Consumidor.Remover(recursoConsumidor.Consumidor.Identificador);
                }
                break;
        }

        recursoConsumidor.Desabilitar();
    }
    #endregion
    
    #region NormalizarStatus
    private void NormalizarStatus(RecursoConsumidor recursoConsumidor)
    {
        if (recursoConsumidor.Status is not (EnumStatusRecursoConsumidor.Habilitado 
                                             or EnumStatusRecursoConsumidor.Desabilitado))
        { 
            recursoConsumidor.Desabilitar();
        }
    }
    #endregion

    #region VerificarSeAtingiu
    private void VerificarSeAtingiu(Recurso recurso, int totalCosumidores)
    {
        if (recurso.Consumidor.TotalHabilitados is 0)
        {
            return;
        }
        
        var porcentagemAtualizada = PorcentagemHelper.Calcular(recurso.Consumidor.TotalHabilitados, totalCosumidores);

        if (porcentagemAtualizada > recurso.Porcentagem.Alvo)
        {
            recurso.Porcentagem.Atingir(porcentagemAtualizada);
        }
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
