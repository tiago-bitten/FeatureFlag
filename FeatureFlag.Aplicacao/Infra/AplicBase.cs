using FeatureFlag.Domain.Infra;

namespace FeatureFlag.Aplicacao.Infra;

public abstract class AplicBase
{
    public Task IniciarTransacaoAsync()
    {
        return Task.CompletedTask;
    }
    
    public Task PersistirTransacaoAsync()
    {
        return Task.CompletedTask;
    }
}