namespace FeatureFlag.Dominio.Dtos;

public record ControleAcessoConsumidorResponse(string IdentificadorConsumidor,
                                               string IdentificadorRecurso,
                                               EnumTipoControle Tipo);
