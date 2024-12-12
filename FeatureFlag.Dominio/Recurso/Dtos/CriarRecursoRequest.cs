namespace FeatureFlag.Domain.Dtos;

public record AdicionarRecursoRequest(
    string Identificador,
    string Descricao,
    decimal Porcentagem);
