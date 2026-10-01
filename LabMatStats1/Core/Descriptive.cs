using LabMatStats1.Models;

namespace LabMatStats1.Core;

public static class Descriptive
{
    public static double Mean(IReadOnlyList<double> x) => x.Sum() / x.Count;

    /// <summary>d = Σ(xi − x̄)² / n</summary>
    public static double DispersionByDefinition(IReadOnlyList<double> x)
    {
        double m = Mean(x);
        return x.Sum(v => (v - m) * (v - m)) / x.Count;
    }

    /// <summary>d = (x²)‾ − (x̄)²</summary>
    public static double DispersionByFormula(IReadOnlyList<double> x)
    {
        double m = Mean(x);
        return x.Sum(v => v * v) / x.Count - m * m;
    }

    /// <summary>s² = n / (n − 1) · d</summary>
    public static double CorrectedDispersion(IReadOnlyList<double> x)
        => DispersionByDefinition(x) * x.Count / (x.Count - 1);

    /// <summary>Мода (може бути кілька). Якщо всі значення унікальні — моди немає.</summary>
    public static IReadOnlyList<double> Modes(IReadOnlyList<double> x)
    {
        var groups = x.GroupBy(v => v).Select(g => (Value: g.Key, Count: g.Count())).ToList();
        int max = groups.Max(g => g.Count);
        if (max == 1) return Array.Empty<double>();
        return groups.Where(g => g.Count == max).Select(g => g.Value).OrderBy(v => v).ToList();
    }

    /// <summary>Медіана: при парному n — (x_m + x_{m+1}) / 2. Збігається з P50.</summary>
    public static double Median(IReadOnlyList<double> sorted) => Percentiles.Percentile(sorted, 50);

    public static Characteristics Describe(IReadOnlyList<double> sorted)
    {
        double d = DispersionByDefinition(sorted);
        double s2 = CorrectedDispersion(sorted);
        return new Characteristics(
            Mean(sorted),
            d,
            DispersionByFormula(sorted),
            Math.Sqrt(d),
            s2,
            Math.Sqrt(s2),
            Modes(sorted),
            Median(sorted));
    }
}