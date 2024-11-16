namespace FeatureFlag.Shared.Extensions;

public static class ListExtensions
{
    private static readonly Random Random = new();

    #region Embaralhar

    #region Fisher-Yates Shuffle
    public static IList<T> EmbaralharFisherYates<T>(this IList<T> list)
    {
        var shuffledList = new List<T>(list);

        for (var i = shuffledList.Count - 1; i > 0; i--)
        {
            var j = Random.Next(0, i + 1);
            (shuffledList[i], shuffledList[j]) = (shuffledList[j], shuffledList[i]);
        }

        return shuffledList;
    }
    #endregion

    #region Sattolo Cycle
    public static IList<T> EmbaralharSattoloCycle<T>(this IList<T> list)
    {
        var shuffledList = new List<T>(list);

        for (var i = shuffledList.Count - 1; i > 0; i--)
        {
            var j = Random.Next(0, i);
            (shuffledList[i], shuffledList[j]) = (shuffledList[j], shuffledList[i]);
        }

        return shuffledList;
    }
    #endregion

    #region OrderBy Shuffle
    public static IList<T> EmbaralharOrderBy<T>(this IList<T> list)
    {
        return list.OrderBy(_ => Random.Next()).ToList();
    }
    #endregion

    #endregion
}