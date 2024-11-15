namespace FeatureFlag.Domain.Dtos;

public record RecursoResponse(
    string Identificador,
    string Descricao,
    decimal Porcentagem);
