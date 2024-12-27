namespace FeatureFlag.Domain.Dtos;

public record AlterarRecursoRequest(string Identificador,
                                    string NovoIdentificador,
                                    string Descricao,
                                    decimal Porcentagem);
