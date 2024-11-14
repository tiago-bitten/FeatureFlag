namespace FeatureFlag.Dominio.RecursoConsumidor.Dtos;

public record RecursoConsumidorResponse(
    string Recurso,
    string Identificador,
    bool Habilitado);