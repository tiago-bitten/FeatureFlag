namespace FeatureFlag.Shared.Helpers;

public static class PorcentagemHelper
{
    public static bool Validar(decimal valor)
    {
        return valor is >= 0 and <= 100;
    }
}