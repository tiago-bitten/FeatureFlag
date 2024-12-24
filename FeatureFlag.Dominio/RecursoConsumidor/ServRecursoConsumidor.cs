using FeatureFlag.Domain;
using FeatureFlag.Dominio.Dtos;
using FeatureFlag.Dominio.Infra;
using FeatureFlag.Shared.Extensions;
using FeatureFlag.Shared.Helpers;

namespace FeatureFlag.Dominio;

public class ServRecursoConsumidor : ServBase<RecursoConsumidor, IRepRecursoConsumidor>, IServRecursoConsumidor
{
    #region Ctor
    private readonly IRepConsumidor _repConsumidor;
    private readonly IRepControleAcessoConsumidor _repControleAcessoConsumidor;
    private readonly IServRecurso _servRecurso;

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
    public async Task AtualizarStatusAsync(RecursoConsumidor recursoConsumidor)
    {
        var recurso = await _servRecurso.Repositorio.RecuperarPorIdentificadorAsync(recursoConsumidor.Recurso.Identificador);
        var totalConsumidores = await _repConsumidor.CountAsync();
        var totalConsumidoresHabilitados = recurso.Consumidor.TotalHabilitados;

        var porcentagemAtual = PorcentagemHelper.Calcular(totalConsumidoresHabilitados, totalConsumidores);

        switch (porcentagemAtual.CompareTo(recurso.Porcentagem))
        {
            case < 0:
                HabilitarConsumidor(recursoConsumidor, recurso);
                break;
            
            case > 0:
                DesabilitarConsumidor(recursoConsumidor, recurso);
                break;
            
            default:
                NormalizarStatus(recursoConsumidor);
                break;
        }
        
        await AlterarAsync(recursoConsumidor);
        await _servRecurso.AlterarAsync(recurso);
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
        if (recursoConsumidor.Status == EnumStatusRecursoConsumidor.Desabilitado)
            return;
        
        recursoConsumidor.Desabilitar();
        recurso.Consumidor.Remover(recursoConsumidor.Consumidor.Identificador);
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
