namespace FeatureFlag.Dominio.Dtos;

public record ControleAcessoConsumidorResponse(
    string IdentificadorConsumidor,
    List<string> IdentificadoresRecursos,
    EnumTipoControle Tipo);
