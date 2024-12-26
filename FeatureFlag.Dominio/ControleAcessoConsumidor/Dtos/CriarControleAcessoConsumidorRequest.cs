namespace FeatureFlag.Dominio.Dtos;

public record CriarControleAcessoConsumidorRequest(string IdentificadorConsumidor,
                                                   string IdentificadorRecurso,
                                                   EnumTipoControle Tipo);
