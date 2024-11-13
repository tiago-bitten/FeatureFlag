namespace FeatureFlag.Aplicacao.Dtos;

public record RecursoConsumidorResponse(
    string Recurso,
    string Identificador,
    bool Habilitado);
