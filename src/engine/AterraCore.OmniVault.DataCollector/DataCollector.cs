// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.DataCollector;

namespace AterraCore.OmniVault.DataCollector;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class DataCollector : IDataCollector {
    public int Fps { get; internal set; }
    public int FpsMin { get; internal set; }
    public int FpsMax { get; internal set; }
    public double Tps { get; internal set; }
    public double DeltaTps { get; internal set; }
}
