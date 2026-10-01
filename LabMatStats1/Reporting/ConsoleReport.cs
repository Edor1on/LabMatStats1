using LabMatStats1.Models;

namespace LabMatStats1.Reporting;

public static class ConsoleReport
{
    public static void PrintHeader(string title)
    {
        Console.WriteLine();
        Console.WriteLine($"=== {title} ===");
    }

    public static void PrintVariationSeries(IReadOnlyList<double> sorted)
    {
        Console.WriteLine(string.Join(", ", sorted));
        Console.WriteLine($"Обсяг вибірки n = {sorted.Count}");
        Console.WriteLine($"Найменше = {sorted[0]}, найбільше = {sorted[^1]}, розмах R = {sorted[^1] - sorted[0]}");
    }

    public static void PrintFrequencyTable(IReadOnlyList<FrequencyRow> rows, int n)
    {
        Console.WriteLine($"{"xi",-8}{"ni",-8}{"wi",-10}");
        foreach (var r in rows)
            Console.WriteLine($"{r.X,-8}{r.N,-8}{r.W,-10:F4}");
        Console.WriteLine($"Контроль: Σni = {rows.Sum(r => r.N)} (n = {n}), Σwi = {rows.Sum(r => r.W):F4}");
    }

    public static void PrintPolygonPoints(IReadOnlyList<FrequencyRow> rows)
    {
        Console.WriteLine("Полігон частот:           " + string.Join("  ", rows.Select(r => $"({r.X}; {r.N})")));
        Console.WriteLine("Полігон відносних частот: " + string.Join("  ", rows.Select(r => $"({r.X}; {r.W:F4})")));
    }

    public static void PrintCdf(IEnumerable<string> lines)
    {
        Console.WriteLine("F*(x) =");
        foreach (var line in lines)
            Console.WriteLine("   " + line);
    }

    public static void PrintCharacteristics(Characteristics c)
    {
        Console.WriteLine($"Вибіркове середнє x̄            = {c.Mean:F4}");
        Console.WriteLine($"Дисперсія (за означенням)      = {c.DispersionByDefinition:F4}");
        Console.WriteLine($"Дисперсія ((x²)‾ − (x̄)²)       = {c.DispersionByFormula:F4}");
        Console.WriteLine($"Середнє квадратичне відхилення = {c.Sigma:F4}");
        Console.WriteLine($"Виправлена дисперсія s²        = {c.CorrectedDispersion:F4}");
        Console.WriteLine($"Виправлене відхилення s        = {c.CorrectedSigma:F4}");
        Console.WriteLine($"Мода Mo                        = {FormatModes(c.Modes)}");
        Console.WriteLine($"Медіана Me                     = {c.Median:F4}");
        Console.WriteLine($"Коефіцієнт варіації V          = {c.Sigma / c.Mean * 100:F4} %");
        Console.WriteLine(SymmetryNote(c));
    }

    public static void PrintQuartiles(double q1, double q3, double iqr)
    {
        Console.WriteLine($"Q1  = {q1:F4}");
        Console.WriteLine($"Q3  = {q3:F4}");
        Console.WriteLine($"IQR = {iqr:F4}");
    }

    /// <summary>Таблиця порівняння вихідної та перерахованої вибірки (пункт 9).</summary>
    public static void PrintComparison(Characteristics a, int na, Characteristics b, int nb)
    {
        string h1 = $"Вихідна (n = {na})";
        string h2 = $"Перерахована (n = {nb})";
        Console.WriteLine($"{"Характеристика",-32}{h1,-20}{h2,-24}{"Різниця, %",-12}");

        Row("Вибіркове середнє x̄", a.Mean, b.Mean);
        Row("Вибіркова дисперсія d̄", a.DispersionByDefinition, b.DispersionByDefinition);
        Row("Середнє квадратичне σ̄", a.Sigma, b.Sigma);
        Row("Медіана Me", a.Median, b.Median);

        string moA = FormatModes(a.Modes), moB = FormatModes(b.Modes);
        string diff = a.Modes.Count == 1 && b.Modes.Count == 1
            ? $"{Diff(a.Modes[0], b.Modes[0]):F4}"
            : "—";
        Console.WriteLine($"{"Мода Mo",-32}{moA,-20}{moB,-24}{diff,-12}");

        static void Row(string name, double x, double y)
            => Console.WriteLine($"{name,-32}{x,-20:F4}{y,-24:F4}{Diff(x, y),-12:F4}");
    }

    private static double Diff(double original, double recalculated)
        => original == 0 ? double.NaN : Math.Abs(recalculated - original) / Math.Abs(original) * 100.0;

    private static string FormatModes(IReadOnlyList<double> modes)
        => modes.Count == 0 ? "немає" : string.Join("; ", modes.Select(m => m.ToString("F4")));

    private static string SymmetryNote(Characteristics c)
    {
        string mo = c.Modes.Count == 0 ? "—" : c.Modes[0].ToString("F4");
        bool symmetric = c.Modes.Count > 0
            && Math.Abs(c.Mean - c.Median) < 1e-4
            && Math.Abs(c.Mean - c.Modes[0]) < 1e-4;
        return $"x̄ = {c.Mean:F4}, Mo = {mo}, Me = {c.Median:F4} → "
            + (symmetric ? "розподіл симетричний" : "строгої симетричності немає");
    }
}