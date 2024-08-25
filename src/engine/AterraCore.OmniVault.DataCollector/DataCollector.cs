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
    public string FpsAverageString { get; private set; } = string.Empty;

    public int Tps { get; internal set; }
    public int TpsMin { get; internal set; }
    public int TpsMax { get; internal set; }
    public double TpsAverage { get; private set; }
    public string TpsAverageString { get; private set; } = string.Empty;

    public double DeltaTps { get; internal set; }

    private const int MaxHistorySize = 60 * 1000;
    #region FpsAverageWatch
    private readonly HashSet<int> _fpsHistory = new(MaxHistorySize);
    private readonly Stopwatch _fpsAverageWatch = Stopwatch.StartNew();
    internal void UpdateFpsAverage(int fps) {
        _fpsHistory.Add(fps);

        if (_fpsAverageWatch.ElapsedMilliseconds < 1000) return;
        FpsAverage = _fpsHistory.Average();
        FpsAverageString = $"{FpsAverage:N2}";

        if (_fpsAverageWatch.ElapsedMilliseconds < MaxHistorySize) _fpsHistory.Clear();
        _fpsAverageWatch.Restart();
    }
    #endregion
    #region TpsAverageWatch
    private readonly HashSet<int> _tpsHistory = new(MaxHistorySize);
    private readonly Stopwatch _tpsAverageWatch = Stopwatch.StartNew();
    internal void UpdateTpsAverage(int tps) {
        _tpsHistory.Add(tps);

        if (_tpsAverageWatch.ElapsedMilliseconds < 1000) return;
        TpsAverage = _tpsHistory.Average();
        TpsAverageString = $"{TpsAverage:N2}";

        if (_tpsAverageWatch.ElapsedMilliseconds < MaxHistorySize) _tpsHistory.Clear();
        _tpsAverageWatch.Restart();
    }
    #endregion
}
