namespace FeatureFlag.Aplicacao.Infra;

public interface IAplicBase
{
    Task IniciarTransacaoAsync();
    Task PersistirTransacaoAsync();
}