namespace FeatureFlag.Dominio.Dtos;

public record CriarControleAcessoConsumidorRequest(
    string IdentificadorConsumidor,
    List<string> IdentificadoresRecursos,
    EnumTipoControle Tipo);
