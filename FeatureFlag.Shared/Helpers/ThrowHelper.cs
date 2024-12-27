namespace FeatureFlag.Shared.Helpers;

public static class ThrowHelper
{
    public static void NullException(string mensagem)
    {
        throw new FeatureFlagAppExeception(EnumTipoExcecao.EntidadeNull, mensagem);
    }

    public static void BusinessException(string mensagem)
    {
        throw new FeatureFlagAppExeception(EnumTipoExcecao.BusinessException, mensagem);
    }
    
    public static void RequiredFieldException(string campo)
    {
        throw new FeatureFlagAppExeception(EnumTipoExcecao.RequiredField, $"Campo {campo} é obrigatório.");
    }
}

#region FeatureFlagAppExeception
public class FeatureFlagAppExeception(EnumTipoExcecao codigoErro, string mensagem) : Exception(mensagem)
{
    private EnumTipoExcecao CodigoErro { get; } = codigoErro;
}
#endregion

#region EnumTipoExcecao

public enum EnumTipoExcecao
{
    EntidadeNull = 1,
    BusinessException = 2,
    RequiredField = 3
}
#endregion