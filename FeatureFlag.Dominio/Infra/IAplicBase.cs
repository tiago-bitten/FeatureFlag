namespace FeatureFlag.Dominio.Infra;

public interface IAplicBase
{
    Task IniciarTransacaoAsync();
    Task PersistirTransacaoAsync();
}