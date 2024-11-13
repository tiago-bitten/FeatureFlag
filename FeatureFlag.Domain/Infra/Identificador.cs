namespace FeatureFlag.Domain.Infra;

public abstract class Identificador
{
    public Guid Id { get; private set; } = Guid.NewGuid();
}