namespace FeatureFlag.Aplicacao.Infra;

public abstract class AplicBase : IAplicBase
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