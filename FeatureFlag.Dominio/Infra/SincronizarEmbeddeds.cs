namespace FeatureFlag.Dominio.Infra;

public record SincronizarEmbeddeds<T>(T Entidade) where T : IdentificadorObjectId;