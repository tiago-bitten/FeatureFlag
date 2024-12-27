using FeatureFlag.Shared.Helpers;

namespace FeatureFlag.Shared.Extensions;

public static class CoringaExtensions
{
    public static void ThrowIfNull<T>(this T? obj, string mensagem = "Classe não existe.") where T : class
    {
        if (obj is null)
            ThrowHelper.NullException(mensagem);
    }
}
    