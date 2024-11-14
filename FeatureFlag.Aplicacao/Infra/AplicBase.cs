using AutoMapper;
using FeatureFlag.Dominio.Infra;

namespace FeatureFlag.Aplicacao.Infra;

public abstract class AplicBase : IAplicBase
{
    protected readonly IMapper Mapper;

    protected AplicBase(IMapper mapper)
    {
        Mapper = mapper;
    }

    public Task IniciarTransacaoAsync()
    {
        return Task.CompletedTask;
    }
    
    public Task PersistirTransacaoAsync()
    {
        return Task.CompletedTask;
    }

    public TSaida Mapear<TEntrada, TSaida>(TEntrada entrada)
    {
        return Mapper.Map<TSaida>(entrada);
    }
}