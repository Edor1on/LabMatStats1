namespace LabMatStats1.Models;

/// <summary>Числові характеристики вибірки (пункт 6 завдання).</summary>
public record Characteristics(
    double Mean,                    // вибіркове середнє x̄
    double DispersionByDefinition,  // d = Σ(xi − x̄)² / n
    double DispersionByFormula,     // d = (x²)‾ − (x̄)²
    double Sigma,                   // σ = √d
    double CorrectedDispersion,     // s²
    double CorrectedSigma,          // s
    IReadOnlyList<double> Modes,    // мода (може бути кілька або жодної)
    double Median);                 // медіана