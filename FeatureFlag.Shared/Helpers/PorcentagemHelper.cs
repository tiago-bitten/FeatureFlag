using System.Numerics;

namespace FeatureFlag.Shared.Helpers;

public static class PorcentagemHelper
{
    public static bool Validar(decimal valor)
    {
        return valor is >= 0 and <= 100;
    }

    public static decimal Calcular<T>(T parcial, T total) where T : INumber<T>
    {
        if (total == T.Zero)
            throw new DivideByZeroException("O total não pode ser zero.");

        if (parcial == T.Zero)
            return 0;

        var parcialDecimal = decimal.CreateChecked(parcial);
        var totalDecimal = decimal.CreateChecked(total);
        
        return parcialDecimal / totalDecimal * 100;
    }
}