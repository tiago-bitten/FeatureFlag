namespace FeatureFlag.Shared.Extensions;

public static class CoringaExtensions
{
    public static void ExcecaoSeNull<T>(this T obj, string mensagem)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(mensagem);
        }
    }
}