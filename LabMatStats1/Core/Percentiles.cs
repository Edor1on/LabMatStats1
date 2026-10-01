namespace LabMatStats1.Core;

public static class Percentiles
{
    /// <summary>
    /// Процентиль P_p за методикою з прикладу викладача:
    /// L = (n + 1) * p / 100; якщо L дробове — лінійна інтерполяція між x_k і x_{k+1}.
    /// Вхідний масив має бути відсортований за зростанням.
    /// </summary>
    public static double Percentile(IReadOnlyList<double> sorted, double p)
    {
        int n = sorted.Count;
        double l = Math.Round((n + 1) * p / 100.0, 10);

        if (l <= 1) return sorted[0];
        if (l >= n) return sorted[n - 1];

        int k = (int)Math.Floor(l);
        double frac = l - k;
        // x_k = sorted[k - 1], x_{k+1} = sorted[k]
        return sorted[k - 1] + frac * (sorted[k] - sorted[k - 1]);
    }

    public static double Q1(IReadOnlyList<double> sorted) => Percentile(sorted, 25);
    public static double Q3(IReadOnlyList<double> sorted) => Percentile(sorted, 75);
    public static double Iqr(IReadOnlyList<double> sorted) => Q3(sorted) - Q1(sorted);
}