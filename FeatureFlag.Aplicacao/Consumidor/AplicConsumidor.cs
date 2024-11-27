using AutoMapper;
using FeatureFlag.Aplicacao.Infra;
using FeatureFlag.Dominio;
using FeatureFlag.Dominio.Dtos;

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

    public async Task<ConsumidorResponse> AdicionarAsync(CriarConsumidorRequest request)
    {
        var consumidor = Mapper.Map<Consumidor>(request);

        await IniciarTransacaoAsync();
        await _servConsumidor.AdicionarAsync(consumidor);
        await PersistirTransacaoAsync();
        
        var response = Mapper.Map<ConsumidorResponse>(consumidor);
        
        return response;
    }
}