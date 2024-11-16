namespace FeatureFlag.Shared.Helpers;

public static class NumeroHelper
{
    public static decimal Arredondar(decimal valor, int casasDecimais = 0)
    {
        return Math.Round(valor, casasDecimais, MidpointRounding.ToEven);
    }
    
    public static double Arredondar(double valor, int casasDecimais = 0)
    {
        return Math.Round(valor, casasDecimais, MidpointRounding.ToEven);
    }
}