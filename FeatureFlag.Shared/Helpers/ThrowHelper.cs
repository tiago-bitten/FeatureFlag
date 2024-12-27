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
    
    public static void FieldRequiredException(string campo)
    {
        throw new FeatureFlagAppExeception(EnumTipoExcecao.FieldRequired, $"Campo {campo} é obrigatório");
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
    BusinessException = 2,
    FieldRequired = 3
}
#endregion