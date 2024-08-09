// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.DataCollector;
using Serilog;
using System.Numerics;

namespace AterraCore.OmniVault.DataCollector;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class DataCollector(ILogger logger) : IDataCollector {
    public double Tps { get; internal set; }
    public double DeltaTps { get; internal set; }
}
