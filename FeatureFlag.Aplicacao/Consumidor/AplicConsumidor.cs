using AutoMapper;
using FeatureFlag.Aplicacao.Infra;
using FeatureFlag.Domain;
using FeatureFlag.Domain.Dtos;
using FeatureFlag.Dominio;
using FeatureFlag.Dominio.Dtos;
using FeatureFlag.Dominio.Infra;
using FeatureFlag.Shared.Extensions;

namespace FeatureFlag.Aplicacao;

public class AplicConsumidor : AplicBase, IAplicConsumidor
{
    #region Ctor
    private readonly IServConsumidor _servConsumidor;
    private readonly IServRecursoConsumidor _servRecursoConsumidor;
    private readonly IServControleAcessoConsumidor _servControleAcessoConsumidor;
    
    public AplicConsumidor(IMapper mapper, 
                           IServConsumidor servConsumidor, 
                           IServRecursoConsumidor servRecursoConsumidor,
                           IServControleAcessoConsumidor servControleAcessoConsumidor)
        : base(mapper)
    {
        _servConsumidor = servConsumidor;
        _servRecursoConsumidor = servRecursoConsumidor;
        _servControleAcessoConsumidor = servControleAcessoConsumidor;
    }
    #endregion

    #region AdicionarAsync
    public async Task<ConsumidorResponse> AdicionarAsync(AdicionarConsumidorRequest request)
    {
        var consumidor = Mapper.Map<Consumidor>(request);

        await IniciarTransacaoAsync();
        await _servConsumidor.AdicionarAsync(consumidor);
        await PersistirTransacaoAsync();
        
        var response = Mapper.Map<ConsumidorResponse>(consumidor);
        
        return response;
    }
    #endregion
    
    #region AtualizarAsync
    public async Task<ConsumidorResponse> AlterarAsync(string identificador, AlterarConsumidorRequest request)
    {
        var consumidor = await _servConsumidor.Repositorio.RecuperarPorIdentificadorAsync(identificador);
        consumidor.ThrowIfNull("Consumidor foi não encontrado.");
        
        Mapper.Map(request, consumidor);

        await _servConsumidor.AtualizarAsync(consumidor);
        await SincronizarEmbeddedsAsync(new SincronizarEmbeddeds<Consumidor>(consumidor));
        
        var response = Mapper.Map<ConsumidorResponse>(consumidor);
        
        return response;
    }
    #endregion
    
    #region RecuperarPorIdentificadorAsync
    public async Task<ConsumidorResponse> RecuperarPorIdentificadorAsync(string identificador)
    {
        var consumidor = await _servConsumidor.Repositorio.RecuperarPorIdentificadorAsync(identificador);
        consumidor.ThrowIfNull("Consumidor não foi encontrado.");
        
        var response = Mapper.Map<ConsumidorResponse>(consumidor);
        
        return response;
    }
    #endregion
    
    #region SincronizarEmbeddedsAsync
    public async Task SincronizarEmbeddedsAsync(SincronizarEmbeddeds<Consumidor> consumidorAtualizado)
    {
        var consumidor = consumidorAtualizado.Entidade;
        
        var recursosConsumidores = await _servRecursoConsumidor.Repositorio.RecuperarPorConsumidorAsync(consumidor.Id);
        _servConsumidor.SincronizarRecursoConsumidores(consumidor, recursosConsumidores);
        await _servRecursoConsumidor.AtualizarVariosAsync(recursosConsumidores);
        
        var controleAcessosConsumidores = await _servControleAcessoConsumidor.Repositorio.RecuperarPorConsumidorAsync(consumidor.Id);
        _servConsumidor.SincronizarControleAcessoConsumidores(consumidor, controleAcessosConsumidores);
        await _servControleAcessoConsumidor.AtualizarVariosAsync(controleAcessosConsumidores);
    }
    #endregion
}