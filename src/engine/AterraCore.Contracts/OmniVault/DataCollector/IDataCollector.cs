// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Contracts.OmniVault.DataCollector;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IDataCollector {
    int Fps { get;}
    int FpsMin { get;}
    int FpsMax { get;}
    double FpsAverage { get; }
    string FpsAverageString { get; }
    
    int Tps { get;}
    double TpsAverage { get; }
    string TpsAverageString { get; }
    double DeltaTps { get;}
}
