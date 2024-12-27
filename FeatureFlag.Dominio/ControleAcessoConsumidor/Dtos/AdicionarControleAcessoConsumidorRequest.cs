namespace FeatureFlag.Dominio.Dtos;

public record AdicionarControleAcessoConsumidorRequest(string IdentificadorConsumidor,
                                                   string IdentificadorRecurso,
                                                   EnumTipoControle Tipo);
