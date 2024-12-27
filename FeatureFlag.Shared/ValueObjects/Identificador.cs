using FeatureFlag.Shared.Helpers;

namespace FeatureFlag.Shared.ValueObjects;

public sealed class Identificador
{
    private const int TamanhoMinimo = 1;
    private const int TamanhoMaximo = 25;

    public string Valor { get; }

    public Identificador(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            ThrowHelper.RequiredFieldException("Identificador");

        if (valor.Length is < TamanhoMinimo or > TamanhoMaximo)
            ThrowHelper.BusinessException("O identificador deve ter entre 1 e 25 caracteres.");

        Valor = valor;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Identificador outro)
            return false;

        return Valor == outro.Valor;
    }

    public override int GetHashCode() => Valor.GetHashCode();

    public override string ToString() => Valor;

    public static implicit operator string(Identificador identificador) => identificador.Valor;

    public static implicit operator Identificador(string valor) => new(valor);
}