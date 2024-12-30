using FeatureFlag.Domain;
using FeatureFlag.Dominio.Dtos;
using FeatureFlag.Dominio.Infra;
using MongoDB.Bson;

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
    public async Task AtualizarDisponibilidadeAsync(RecursoConsumidor recursoConsumidor, Recurso recurso, Consumidor consumidor)
    {
        var totalConsumidores = await _repConsumidor.CountAsync();
        var porcentagemAtual = await _servRecurso.CalcularPorcentagemAsync(recurso, totalConsumidores);

        switch (porcentagemAtual.CompareTo(recurso.Porcentagem))
        {
            case < 0:
                HabilitarConsumidor(recursoConsumidor, recurso, consumidor);
                break;
            
            case > 0:
                DesabilitarConsumidor(recursoConsumidor, recurso, consumidor);
                break;
            
            default:
                NormalizarStatus(recursoConsumidor, recurso, consumidor);
                break;
        }
        
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
        recursoConsumidor.Congelar();
        recurso.Consumidor.Adicionar();
    }
    #endregion
    
    #region DesabilitarConsumidor
    private void DesabilitarConsumidor(RecursoConsumidor recursoConsumidor, Recurso recurso, Consumidor consumidor)
    {
        if (recursoConsumidor.Status is EnumStatusRecursoConsumidor.Desabilitado)
        {
            return;
        }
        
        recursoConsumidor.Desabilitar();
        recurso.Consumidor.Remover();
    }
    #endregion
    
    #region NormalizarStatus
    private void NormalizarStatus(RecursoConsumidor recursoConsumidor, Recurso recurso, Consumidor consumidor)
    {
        if (recursoConsumidor.Status is EnumStatusRecursoConsumidor.Habilitado or EnumStatusRecursoConsumidor.Desabilitado)
        {
            return;
        }
        recursoConsumidor.Desabilitar();
    }
    #endregion
    #endregion
    
    #region RemoverPorRecursoAsync
    public async Task RemoverPorRecursoAsync(Recurso recurso)
    {
        var recursosConsumidores = await Repositorio.RecuperarPorRecursoAsync(recurso.Id);
        foreach (var recursoConsumidor in recursosConsumidores)
        {
            await RemoverAsync(recursoConsumidor);
        }
    }
    #endregion
    
    #region RemoverPorConsumidorAsync
    public async Task RemoverPorConsumidorAsync(Consumidor consumidor)
    {
        var recursosConsumidores = await Repositorio.RecuperarPorConsumidorAsync(consumidor.Id);
        foreach (var recursoConsumidor in recursosConsumidores)
        {
            await RemoverAsync(recursoConsumidor);
        }
    }
    #endregion
    
    #region DescongelarTodosAsync
    public async Task DescongelarTodosPorRecursoAsync(Recurso recurso)
    {
        var recursosConsumidores = await Repositorio.RecuperarPorRecursoAsync(recurso.Id);
        foreach (var recursoConsumidor in recursosConsumidores)
        {
            recursoConsumidor.Descongelar();
            await AtualizarAsync(recursoConsumidor);
        }
    }
    #endregion
    
    #region RetornarComControleAcesso
    public RecursoConsumidorResponse RetornarComControleAcesso(Consumidor consumidor, Recurso recurso)
    {
        var controleAcesso = consumidor.ControleAcessos
            .FirstOrDefault(x => x.Recurso.Id == recurso.Id);

        return controleAcesso?.Tipo switch
        {
            EnumTipoControle.Whitelist => RecursoConsumidorResponse.Ativo(consumidor.Identificador, recurso.Identificador),
            EnumTipoControle.Blacklist => RecursoConsumidorResponse.Desabilitado(consumidor.Identificador, recurso.Identificador),
            _ => throw new NotImplementedException()
        };
    }
    #endregion
    
    #region Retornar 0% ou 100%
    #region RetornarCemPorcentoAsync
    public RecursoConsumidorResponse RetornarCemPorcentoAtivo(Consumidor consumidor, Recurso recurso)
    {
        var estaNaBlacklist = consumidor.ControleAcessos
            .FirstOrDefault(x => x.Recurso.Id == recurso.Id && x.Tipo == EnumTipoControle.Blacklist) is not null;
            
        return RetornarPermissao(estaNaBlacklist, EnumTipoControle.Blacklist, consumidor, recurso);
    }
    #endregion
        
    #region RetornarZeroPorcentoAsync
    public RecursoConsumidorResponse RetornarZeroPorcentoAtivo(Consumidor consumidor, Recurso recurso)
    {
        var estaNaWhitelist = consumidor.ControleAcessos
            .FirstOrDefault(x => x.Recurso.Id == recurso.Id && x.Tipo == EnumTipoControle.Whitelist) is not null;
            
        return RetornarPermissao(estaNaWhitelist, EnumTipoControle.Whitelist, consumidor, recurso);
    }
    #endregion
        
    #region RetornarPermissao
    private RecursoConsumidorResponse RetornarPermissao(bool estaNaLista, EnumTipoControle tipo, Consumidor consumidor, Recurso recurso)
    {
        return tipo switch
        {
            EnumTipoControle.Whitelist => estaNaLista
                ? RecursoConsumidorResponse.Ativo(consumidor.Identificador, recurso.Identificador)
                : RecursoConsumidorResponse.Desabilitado(consumidor.Identificador, recurso.Identificador),

            EnumTipoControle.Blacklist => estaNaLista
                ? RecursoConsumidorResponse.Desabilitado(consumidor.Identificador, recurso.Identificador)
                : RecursoConsumidorResponse.Ativo(consumidor.Identificador, recurso.Identificador),

            _ => throw new NotImplementedException()
        };
    }
    #endregion
    #endregion
}
