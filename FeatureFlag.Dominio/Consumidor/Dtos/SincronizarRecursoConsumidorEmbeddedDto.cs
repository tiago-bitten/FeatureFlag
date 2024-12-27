namespace FeatureFlag.Dominio.Dtos;

public record SincronizarRecursoConsumidorEmbeddedDto(string? IdentificadorRecurso = null,
                                                      EnumStatusRecursoConsumidor? Status = null);