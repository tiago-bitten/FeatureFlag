using AutoMapper;
using FeatureFlag.Aplicacao.Infra;
using FeatureFlag.Dominio;
using FeatureFlag.Dominio.Dtos;
using FeatureFlag.Shared.Extensions;

namespace FeatureFlag.Aplicacao;

public class AplicConsumidor : AplicBase, IAplicConsumidor
{
    #region Ctor
    private readonly IServConsumidor _servConsumidor;
    
    public AplicConsumidor(IMapper mapper, 
                           IServConsumidor servConsumidor)
        : base(mapper)
    {
        _servConsumidor = servConsumidor;
    }
    #endregion

    #region AdicionarAsync
    public async Task<ConsumidorResponse> AdicionarAsync(CriarConsumidorRequest request)
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
    public async Task<ConsumidorResponse> AlterarAsync(AlterarConsumidorRequest request)
    {
        var consumidor = await _servConsumidor.Repositorio.RecuperarPorIdentificadorAsync(request.Identificador);
        consumidor.ThrowIfNull();
        
        var consumidorAlterado = Mapper.Map(request, consumidor);

        await IniciarTransacaoAsync();
        await _servConsumidor.AtualizarAsync(consumidorAlterado);
        await PersistirTransacaoAsync();
        
        var response = Mapper.Map<ConsumidorResponse>(consumidorAlterado);
        
        return response;
    }
    #endregion
}