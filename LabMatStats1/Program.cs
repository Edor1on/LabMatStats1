using System.Globalization;
using System.Text;
using LabMatStats1.Core;
using LabMatStats1.Data;
using LabMatStats1.Reporting;

Console.OutputEncoding = Encoding.UTF8;
CultureInfo.CurrentCulture = new CultureInfo("uk-UA");

const int M = 100;
string outDir = Path.Combine(AppContext.BaseDirectory, "output");
Console.WriteLine("=== 1. Варіант №6: кількість дефектів на 1 м² полотна, n = 74 ===");
Directory.CreateDirectory(outDir);

// ---------- 2. Варіаційний ряд ----------
double[] sorted = Variant6.Values.OrderBy(v => v).ToArray();
int n = sorted.Length;

ConsoleReport.PrintHeader("2. Варіаційний ряд");
ConsoleReport.PrintVariationSeries(sorted);

// ---------- 3. Статистичний розподіл частот і відносних частот ----------
var table = FrequencyTable.Build(sorted);
ConsoleReport.PrintHeader("3. Статистичний розподіл");
ConsoleReport.PrintFrequencyTable(table, n);

// ---------- 4. Полігони частот та відносних частот ----------
ConsoleReport.PrintHeader("4. Полігони");
ConsoleReport.PrintPolygonPoints(table);
Console.WriteLine("Збережено: " + Plots.SaveFrequencyPolygon(table, outDir));
Console.WriteLine("Збережено: " + Plots.SaveRelativeFrequencyPolygon(table, outDir));

// ---------- 5. Емпірична функція розподілу ----------
ConsoleReport.PrintHeader("5. Емпірична функція розподілу");
ConsoleReport.PrintCdf(EmpiricalCdf.Describe(sorted));
Console.WriteLine("Збережено: " + Plots.SaveEmpiricalCdf(table, outDir));

// ---------- 6. Числові характеристики ----------
var original = Descriptive.Describe(sorted);
ConsoleReport.PrintHeader("6. Числові характеристики");
ConsoleReport.PrintCharacteristics(original);

// ---------- 7. Квартилі та IQR ----------
ConsoleReport.PrintHeader("7. Квартилі");
ConsoleReport.PrintQuartiles(Percentiles.Q1(sorted), Percentiles.Q3(sorted), Percentiles.Iqr(sorted));

// ---------- 8. Перерахунок вибірки до m = 100 ----------
double[] recalculated = Resampler.Resample(sorted, M);
ConsoleReport.PrintHeader($"8. Перерахована вибірка (m = {M})");
Console.WriteLine(string.Join(", ", recalculated.Select(v => v.ToString("F4"))));

// ---------- 9. Характеристики перерахованої вибірки та порівняння ----------
var recalcStats = Descriptive.Describe(recalculated);
ConsoleReport.PrintHeader("9. Характеристики перерахованої вибірки");
ConsoleReport.PrintCharacteristics(recalcStats);

ConsoleReport.PrintHeader("9. Порівняльна таблиця");
ConsoleReport.PrintComparison(original, n, recalcStats, M);

Console.WriteLine();
Console.WriteLine("Готово.");