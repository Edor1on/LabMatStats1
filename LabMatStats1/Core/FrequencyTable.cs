using LabMatStats1.Models;

namespace LabMatStats1.Core;

public static class FrequencyTable
{
    public static List<FrequencyRow> Build(IReadOnlyList<double> sorted)
    {
        int n = sorted.Count;
        return sorted
            .GroupBy(v => v)
            .OrderBy(g => g.Key)
            .Select(g => new FrequencyRow(g.Key, g.Count(), (double)g.Count() / n))
            .ToList();
    }
}