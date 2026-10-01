namespace LabMatStats1.Core;

public static class Resampler
{
    /// <summary>
    /// Перерахунок вибірки до обсягу m через квантилі.
    ///
    /// УВАГА: це ПРИПУЩЕННЯ щодо формул (1.17) і (1.18) з методички.
    /// Тут y_j — квантиль рівня p_j = j / (m + 1), j = 1..m, який рахується
    /// за тією самою процентильною формулою L = (n + 1) · p з інтерполяцією.
    /// Якщо в методичці формули інші — міняти потрібно лише цей метод.
    /// </summary>
    public static double[] Resample(IReadOnlyList<double> sorted, int m)
    {
        // Формули (1.17), (1.18) з K = m = 100: y_j = P_j, L_j = (n + 1) * j / K
        var y = new double[m];
        for (int j = 1; j <= m; j++)
        {
            double percent = 100.0 * j / m;   // для m = 100 це просто j
            y[j - 1] = Percentiles.Percentile(sorted, percent);
        }
        return y;
    }
}