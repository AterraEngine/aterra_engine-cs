// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.DataCollector;
using System.Diagnostics;

namespace AterraCore.OmniVault.DataCollector;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class DataCollector : IDataCollector {
    public int Fps { get; internal set; }
    public int FpsMin { get; internal set; }
    public int FpsMax { get; internal set; }
    public double FpsAverage { get; private set; }
    
    public int Tps { get; internal set; }
    public double TpsAverage { get; private set; }
    
    public double DeltaTps { get; internal set; }
    
    private const int MaxHistorySize = 60 * 1000;
    private readonly HashSet<int> _fpsHistory  = new(MaxHistorySize);
    private readonly Stopwatch _fpsAverageWatch = Stopwatch.StartNew();
    internal void UpdateFpsAverage(int fps) {
        _fpsHistory.Add(fps);
        
        if (_fpsAverageWatch.ElapsedMilliseconds < 1000) return;
        FpsAverage = _fpsHistory.Average();
        
        if (_fpsAverageWatch.ElapsedMilliseconds < MaxHistorySize) _fpsHistory.Clear();
        _fpsAverageWatch.Restart();
    }
    
    private readonly HashSet<int> _tpsHistory  = new(MaxHistorySize);
    private readonly Stopwatch _tpsAverageWatch = Stopwatch.StartNew();
    internal void UpdateTpsAverage(int tps) {
        _tpsHistory.Add(tps);
        
        if (_tpsAverageWatch.ElapsedMilliseconds < 1000) return;
        TpsAverage = _tpsHistory.Average();
        
        if (_tpsAverageWatch.ElapsedMilliseconds < MaxHistorySize) _tpsHistory.Clear();
        _tpsAverageWatch.Restart();
    }
}
