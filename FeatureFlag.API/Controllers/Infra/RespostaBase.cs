using System.Text.Json.Serialization;

namespace FeatureFlag.API.Controllers.Infra;

public record RespostaBase<T>(
    bool Sucesso,
    
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    T? Conteudo,
    
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    string? Mensagem,
    
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    int? Total)
    where T : class;

static class RespostaBase
{
    public static RespostaBase<T> Sucesso<T>(T conteudo, string mensagem) where T : class
    {
        return new RespostaBase<T>(Sucesso: true, Conteudo: conteudo, Mensagem: mensagem, Total: null);
    }
    
    public static RespostaBase<dynamic> Sucesso(string mensagem)
    {
        return new RespostaBase<dynamic>(Sucesso: true, Conteudo: null, Mensagem: mensagem, Total: null);
    }

    public static RespostaBase<T> Sucesso<T>(T conteudo, int total, string mensagem) where T : class
    {
        return new RespostaBase<T>(Sucesso: true, Conteudo: conteudo, Mensagem: mensagem, Total: total);
    }

    public static RespostaBase<dynamic> Erro(string mensagem)
    {
        return new RespostaBase<dynamic>(Sucesso: false, Conteudo: null, Mensagem: mensagem, Total: null);
    }
}
