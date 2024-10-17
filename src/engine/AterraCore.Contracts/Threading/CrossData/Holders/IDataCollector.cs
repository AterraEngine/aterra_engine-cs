// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Contracts.Threading.CrossData.Holders;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IDataCollector : ICrossThreadData {
    int Fps { get; set; }
    int FpsMin { get; }
    int FpsMax { get; }
    double FpsAverage { get; set; }
    string FpsAverageString { get; set; }

    int Tps { get; set; }
    int TpsMin { get; }
    int TpsMax { get; }
    double TpsAverage { get; set; }
    string TpsAverageString { get; set; }
    double DeltaTps { get; set; }
}
