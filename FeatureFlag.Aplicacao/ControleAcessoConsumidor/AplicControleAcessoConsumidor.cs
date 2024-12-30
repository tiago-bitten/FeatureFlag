using AutoMapper;
using FeatureFlag.Aplicacao.Infra;
using FeatureFlag.Domain;
using FeatureFlag.Dominio;
using FeatureFlag.Dominio.Dtos;
using FeatureFlag.Shared.Extensions;
using FeatureFlag.Shared.ValueObjects;

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
    public async Task<ControleAcessoConsumidorResponse> AdicionarAsync(AdicionarControleAcessoConsumidorRequest request)
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
                consumidor.AdicionarWhitelist(recurso);
                break;
            case EnumTipoControle.Blacklist:
                consumidor.AdicionarBlacklist(recurso);
                break;
        }
        
        await _servConsumidor.AtualizarAsync(consumidor);
        
        var response = Mapper.Map<ControleAcessoConsumidorResponse>(controleAcessoConsumidor);

        return response;
    }
    #endregion

    #region RecuperarPorConsumidorAsync
    public async Task<List<ControleAcessoConsumidorResponse>> RecuperarPorConsumidorAsync(string identificadorConsumidor)
    {
        var consumidor = await _servConsumidor.Repositorio.RecuperarPorIdentificadorAsync(identificadorConsumidor);
        consumidor.ThrowIfNull("Consumidor não foi encontrado.");
        
        var controlesAcessoConsumidor = await _servControleAcessoConsumidor.Repositorio.RecuperarPorConsumidorAsync(consumidor.Id);
        var response = Mapper.Map<List<ControleAcessoConsumidorResponse>>(controlesAcessoConsumidor);

        return response;
    }
    #endregion
    
    #region RemoverPorRecursoConsumidorAsync
    public async Task RemoverPorRecursoConsumidorAsync(RemoverPorRecursoConsumidorParam param)
    {
        var consumidor = await _servConsumidor.Repositorio.RecuperarPorIdentificadorAsync(param.IdentificadorConsumidor);
        consumidor.ThrowIfNull("Consumidor não foi encontrado.");
        
        var recurso = await _servRecurso.Repositorio.RecuperarPorIdentificadorAsync(param.IdentificadorRecurso); 
        recurso.ThrowIfNull("Recurso não foi encontrado.");
        
        var controleAcessoConsumidor = await _servControleAcessoConsumidor.Repositorio.RecuperarPorRecursoConsumidorAsync(recurso.Id, consumidor.Id);
        controleAcessoConsumidor.ThrowIfNull("Não foi encontrado controle de acesso para o recurso e consumidor informados.");
        
        consumidor.RemoverControleAcesso(recurso.Identificador);
        await _servConsumidor.AtualizarAsync(consumidor);
        
        controleAcessoConsumidor.Inativar();
        await _servControleAcessoConsumidor.AtualizarAsync(controleAcessoConsumidor);
    }
    #endregion
}