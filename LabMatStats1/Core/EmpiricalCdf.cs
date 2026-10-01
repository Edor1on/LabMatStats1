namespace LabMatStats1.Core;

public static class EmpiricalCdf
{
    /// <summary>F*(x) = n_x / n, де n_x — кількість значень, менших за x.</summary>
    public static double Value(IReadOnlyList<double> sorted, double x)
        => (double)sorted.Count(v => v < x) / sorted.Count;

    /// <summary>Аналітичний запис F*(x) за проміжками (праворуч дужка закрита: ≤).</summary>
    public static IEnumerable<string> Describe(IReadOnlyList<double> sorted)
    {
        var rows = FrequencyTable.Build(sorted);
        int n = sorted.Count;

        yield return $"0,  якщо x ≤ {rows[0].X}";

        int cum = 0;
        for (int i = 0; i < rows.Count; i++)
        {
            cum += rows[i].N;
            string value = cum == n ? "1" : $"{cum}/{n} = {(double)cum / n:F4}";

            if (i < rows.Count - 1)
                yield return $"{value},  якщо {rows[i].X} < x ≤ {rows[i + 1].X}";
            else
                yield return $"{value},  якщо x > {rows[i].X}";
        }
    }
}