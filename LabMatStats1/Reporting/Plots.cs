using LabMatStats1.Models;

namespace LabMatStats1.Reporting;

/// <summary>Графіки через ScottPlot 5 (NuGet: ScottPlot). Зберігаються у PNG.</summary>
public static class Plots
{
    public static string SaveFrequencyPolygon(IReadOnlyList<FrequencyRow> rows, string dir)
    {
        double[] xs = rows.Select(r => r.X).ToArray();
        double[] ys = rows.Select(r => (double)r.N).ToArray();
        return SaveLine(xs, ys, "Полігон частот", "xi", "ni",
            Path.Combine(dir, "polygon_frequencies.png"));
    }

    public static string SaveRelativeFrequencyPolygon(IReadOnlyList<FrequencyRow> rows, string dir)
    {
        double[] xs = rows.Select(r => r.X).ToArray();
        double[] ys = rows.Select(r => r.W).ToArray();
        return SaveLine(xs, ys, "Полігон відносних частот", "xi", "wi",
            Path.Combine(dir, "polygon_relative_frequencies.png"));
    }

    /// <summary>Графік емпіричної функції розподілу F*(x) — "сходинки".</summary>
    public static string SaveEmpiricalCdf(IReadOnlyList<FrequencyRow> rows, string dir)
    {
        var xs = new List<double>();
        var ys = new List<double>();

        double cum = 0;
        xs.Add(rows[0].X - 1); ys.Add(0);          // зліва F* = 0
        foreach (var r in rows)
        {
            xs.Add(r.X); ys.Add(cum);               // доходимо до xi на попередньому рівні
            cum += r.W;
            xs.Add(r.X); ys.Add(cum);               // стрибок угору після xi
        }
        xs.Add(rows[^1].X + 1); ys.Add(1);          // справа F* = 1

        string path = Path.Combine(dir, "empirical_cdf.png");
        var plt = new ScottPlot.Plot();
        var line = plt.Add.Scatter(xs.ToArray(), ys.ToArray());
        line.LineWidth = 2;
        line.MarkerSize = 0;
        plt.Title("Емпірична функція розподілу F*(x)");
        plt.XLabel("x");
        plt.YLabel("F*(x)");
        plt.Axes.SetLimits(xs.Min() - 0.5, xs.Max() + 0.5, -0.05, 1.1);
        plt.SavePng(path, 800, 600);
        return path;
    }

    private static string SaveLine(double[] xs, double[] ys, string title, string xLabel, string yLabel, string path)
    {
        var plt = new ScottPlot.Plot();
        var line = plt.Add.Scatter(xs, ys);
        line.LineWidth = 2;
        line.MarkerSize = 8;
        plt.Title(title);
        plt.XLabel(xLabel);
        plt.YLabel(yLabel);
        plt.Axes.SetLimits(xs.Min() - 1, xs.Max() + 1, 0, ys.Max() * 1.15);
        plt.SavePng(path, 800, 600);
        return path;
    }
}