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

    public ServRecursoConsumidor(IRepRecursoConsumidor repositorio,
                                 IRepRecurso repRecurso,
                                 IRepConsumidor repConsumidor,
                                 IRepControleAcessoConsumidor repControleAcessoConsumidor)
        : base(repositorio)
    {
        _repRecurso = repRecurso;
        _repConsumidor = repConsumidor;
        _repControleAcessoConsumidor = repControleAcessoConsumidor;
    }
    #endregion
    
    #region RecuperarDisponibilidadeAsync
    public async Task<bool> RecuperarDisponibilidadeAsync(RecursoConsumidor recursoConsumidor)
    {
        await AtualizarStatusAsync(recursoConsumidor);
        return recursoConsumidor.Status == EnumStatusRecursoConsumidor.Habilitado;
    }
    #endregion
    
    #region AtualizarPercentualDisponivelAsync
    public async Task AtualizarStatusAsync(RecursoConsumidor recursoConsumidor)
    {
        var totalConsumidores = await _repConsumidor.CountAsync();
        var totalRecursoConsumidoresHabilitados = Repositorio
            .RecuperarPorStatus(EnumStatusRecursoConsumidor.Habilitado)
            .Count(x => x.Recurso.Identificador == recursoConsumidor.Recurso.Identificador);

        var porcentagemAtual = PorcentagemHelper.Calcular(totalRecursoConsumidoresHabilitados, totalConsumidores);

        switch (porcentagemAtual.CompareTo(recursoConsumidor.Recurso.Porcentagem))
        {
            case < 0:
                HabilitarRecursoConsumidor(recursoConsumidor);
                break;
            
            case > 0:
                DesabilitarRecursoConsumidor(recursoConsumidor);
                break;
            
            default:
                NormalizarStatus(recursoConsumidor);
                break;
        }
        
        Alterar(recursoConsumidor);
    }

    #region HabilitarRecursoConsumidor
    private void HabilitarRecursoConsumidor(RecursoConsumidor recursoConsumidor)
    {
        if (recursoConsumidor.Status == EnumStatusRecursoConsumidor.Habilitado)
            return;
        
        recursoConsumidor.Habilitar();
    }
    #endregion
    
    #region DesabilitarRecursoConsumidor
    private void DesabilitarRecursoConsumidor(RecursoConsumidor recursoConsumidor)
    {
        if (recursoConsumidor.Status == EnumStatusRecursoConsumidor.Desabilitado)
            return;
        
        recursoConsumidor.Desabilitar();
    }
    #endregion
    
    #region NormalizarStatus
    public void NormalizarStatus(RecursoConsumidor recursoConsumidor)
    {
        if (recursoConsumidor.Status is not (EnumStatusRecursoConsumidor.Habilitado 
                                             or EnumStatusRecursoConsumidor.Desabilitado))
        { 
            recursoConsumidor.Desabilitar();
        }
    }
    #endregion
    #endregion
    
    #region RetornarCemPorcentoAsync
    public async Task<RecursoConsumidorResponse> RetornarCemPorcentoAtivoAsync(RecuperarPorRecursoConsumidorParam param)
    {
        var estaNaBlacklist = await _repControleAcessoConsumidor
            .PossuiControleAcessoAsync(param.IdentificadorRecurso, param.IdentificadorConsumidor, EnumTipoControle.Blacklist);
            
        return RetornarPermissao(estaNaBlacklist, EnumTipoControle.Blacklist, param);
    }
    #endregion
        
    #region RetornarZeroPorcentoAsync
    public async Task<RecursoConsumidorResponse> RetornarZeroPorcentoAtivoAsync(RecuperarPorRecursoConsumidorParam param)
    {
        var estaNaWhitelist = await _repControleAcessoConsumidor
            .PossuiControleAcessoAsync(param.IdentificadorRecurso, param.IdentificadorConsumidor, EnumTipoControle.Whitelist);
            
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
}
