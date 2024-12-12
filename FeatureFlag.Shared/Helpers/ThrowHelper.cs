namespace FeatureFlag.Shared.Helpers;

public static class ThrowHelper
{
    public static void Null(string mensagem)
    {
        throw new FeatureFlagAppExeception(EnumTipoExcecao.EntidadeNull, mensagem);
    }
    
    
}

#region FeatureFlagAppExeception
public class FeatureFlagAppExeception : Exception
{
    private EnumTipoExcecao CodigoErro { get; }
    
    public FeatureFlagAppExeception(EnumTipoExcecao codigoErro, string mensagem) : base(mensagem)
    {
        CodigoErro = codigoErro;
    }
}
#endregion

#region EnumTipoExcecao
public enum EnumTipoExcecao
{
    EntidadeNull = 1,
}
#endregion