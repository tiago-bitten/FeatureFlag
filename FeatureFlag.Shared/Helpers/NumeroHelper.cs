using System.Numerics;

namespace FeatureFlag.Shared.Helpers;

public static class NumeroHelper
{
    #region Arredondar
    public static T Arredondar<T>(T valor, int casasDecimais = 0) where T : INumber<T>
    {
        return valor switch
        {
            double d => (T)(dynamic)Math.Round(d, casasDecimais, MidpointRounding.ToEven),
            decimal d => (T)(dynamic)Math.Round(d, casasDecimais, MidpointRounding.ToEven),
            _ => throw new NotSupportedException($"O tipo {typeof(T)} não suporta arredondamento.")
        };
    }
    #endregion

    #region ValorAbsoluto
    public static T ValorAbsoluto<T>(T valor) where T : INumber<T>
    {
        return T.Abs(valor);
    }
    #endregion
}