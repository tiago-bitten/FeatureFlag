using AutoMapper;
using FeatureFlag.Aplicacao.Infra;
using FeatureFlag.Domain;
using FeatureFlag.Dominio;
using FeatureFlag.Dominio.Dtos;
using FeatureFlag.Shared.Extensions;

namespace FeatureFlag.Aplicacao;

public class AplicControleAcessoConsumidor : AplicBase, IAplicControleAcessoConsumidor
{
    #region Ctor
    private readonly IServControleAcessoConsumidor _servControleAcessoConsumidor;
    private readonly IServConsumidor _servConsumidor;
    private readonly IServRecurso _servRecurso;
    
    public AplicControleAcessoConsumidor(IMapper mapper,
                                         IServConsumidor servConsumidor,
                                         IServRecurso servRecurso, 
                                         IServControleAcessoConsumidor servControleAcessoConsumidor)
        : base(mapper)
    {
        _servConsumidor = servConsumidor;
        _servRecurso = servRecurso;
        _servControleAcessoConsumidor = servControleAcessoConsumidor;
    }
    #endregion

    #region AdicionarAsync
    public async Task<ControleAcessoConsumidorResponse> AdicionarAsync(CriarControleAcessoConsumidorRequest request)
    {
        var consumidor = await _servConsumidor.Repositorio.RecuperarPorIdentificadorAsync(request.IdentificadorConsumidor);
        consumidor.ThrowIfNull();
        
        await IniciarTransacaoAsync();
        foreach (var identificadorRecurso in request.IdentificadoresRecursos)
        {
            var recurso = await _servRecurso.Repositorio.RecuperarPorIdentificadorAsync(identificadorRecurso);
            recurso.ThrowIfNull();
            
            var controleAcessoConsumidor = CriarPorTipo(consumidor, recurso, request.Tipo);
            
            await _servControleAcessoConsumidor.AdicionarAsync(controleAcessoConsumidor);
        }
        await PersistirTransacaoAsync();
        
        var response = Mapper.Map<ControleAcessoConsumidorResponse>(request);

        return response;
    }
    
    #region CriarPorTipo
    private ControleAcessoConsumidor CriarPorTipo(Consumidor consumidor, Recurso recurso, EnumTipoControle tipo)
    {
        return tipo switch
        {
            EnumTipoControle.Whitelist => ControleAcessoConsumidor.CriarWhitelist(consumidor.Id, recurso.Id),
            EnumTipoControle.Blacklist => ControleAcessoConsumidor.CriarBlacklist(consumidor.Id, recurso.Id),
            _ => throw new ArgumentOutOfRangeException(nameof(tipo), tipo, null)
        };
    }
    #endregion
    #endregion
}