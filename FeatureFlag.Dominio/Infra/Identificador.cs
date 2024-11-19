namespace FeatureFlag.Dominio.Infra;

public abstract class Identificador
{
    public Guid Id { get; private set; } = Guid.NewGuid();
}