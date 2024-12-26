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
        consumidor.ThrowIfNull("Consumidor não foi encontrado.");
        
        var recurso = await _servRecurso.Repositorio.RecuperarPorIdentificadorAsync(request.IdentificadorRecurso);
        recurso.ThrowIfNull("Recurso não foi encontrado.");

        var controleAcessoConsumidor = new ControleAcessoConsumidor(consumidor, recurso, request.Tipo);
        await _servControleAcessoConsumidor.AdicionarAsync(controleAcessoConsumidor);

        switch (request.Tipo)
        {
            case EnumTipoControle.Whitelist:
                consumidor.AdicionarWhitelist(recurso.Identificador);
                break;
            case EnumTipoControle.Blacklist:
                consumidor.AdicionarBlacklist(recurso.Identificador);
                break;
        }
        
        await _servConsumidor.AtualizarAsync(consumidor);
        
        var response = Mapper.Map<ControleAcessoConsumidorResponse>(controleAcessoConsumidor);

        return response;
    }
    
    #endregion
}