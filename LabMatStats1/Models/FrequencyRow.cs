namespace LabMatStats1.Models;

/// <summary>Рядок статистичного розподілу: варіанта xi, частота ni, відносна частота wi.</summary>
public record FrequencyRow(double X, int N, double W);