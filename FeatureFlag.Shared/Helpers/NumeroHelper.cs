namespace FeatureFlag.Shared.Helpers;

public static class NumeroHelper
{
    #region Arredondar
    public static decimal Arredondar(decimal valor, int casasDecimais = 0)
    {
        return Math.Round(valor, casasDecimais, MidpointRounding.ToEven);
    }
        
    public static double Arredondar(double valor, int casasDecimais = 0)
    {
        return Math.Round(valor, casasDecimais, MidpointRounding.ToEven);
    }
    #endregion
        
    #region ValorAbsoluto
    public static int ValorAbsoluto(int valor)
    {
        return Math.Abs(valor);
    }

    public static decimal ValorAbsoluto(decimal valor)
    {
        return Math.Abs(valor);
    }

    public static double ValorAbsoluto(double valor)
    {
        return Math.Abs(valor);
    }
    #endregion
}