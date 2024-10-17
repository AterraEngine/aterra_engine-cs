// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.AssetVault;
using AterraCore.Common.Attributes.AssetVault;
using AterraCore.Contracts.Threading.CrossData.Holders;
using System.Diagnostics;

namespace AterraLib.Nexities.CrossThreadDataHolders;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[CrossThreadDataHolder<IDataCollector>(StringAssetIdLib.AterraCore.CrossThreadDataHolders.DataCollector)]
public class DataCollector : AssetInstance, IDataCollector {
    private const int MaxHistorySize = 60 * 1000;
    private const int AverageLengthMs = 2500;

    #region FPS
    private int _fps;
    public int Fps {
        get => _fps;
        set {
            _fps = value;
            FpsMin = Math.Min(value, FpsMin);
            FpsMax = Math.Max(value, FpsMax);
            UpdateFpsAverage(value);
        }
    }
    public int FpsMin { get; set; }
    public int FpsMax { get; set; }
    public double FpsAverage { get; set; }
    public string FpsAverageString { get; set; } = string.Empty;

    #region FpsAverageWatch
    private readonly HashSet<int> _fpsHistory = new(MaxHistorySize);
    private readonly Stopwatch _fpsAverageWatch = Stopwatch.StartNew();
    private void UpdateFpsAverage(int fps) {
        _fpsHistory.Add(fps);

        if (_fpsAverageWatch.ElapsedMilliseconds < AverageLengthMs) return;

        FpsAverage = _fpsHistory.Average();
        FpsAverageString = $"{FpsAverage:N2}";

        if (_fpsAverageWatch.ElapsedMilliseconds < MaxHistorySize) _fpsHistory.Clear();
        _fpsAverageWatch.Restart();
    }
    #endregion
    #endregion
    #region TPS
    private int _tps;
    public int Tps {
        get => _tps;
        set {
            _tps = value;
            TpsMin = Math.Min(value, TpsMin);
            TpsMax = Math.Max(value, TpsMax);
            UpdateTpsAverage(value);
        }
    }

    public int TpsMin { get; private set; }
    public int TpsMax { get; private set; }
    public double TpsAverage { get; set; }
    public string TpsAverageString { get; set; } = string.Empty;

    public double DeltaTps { get; set; }

    #region TpsAverageWatch
    private readonly HashSet<int> _tpsHistory = new(MaxHistorySize);
    private readonly Stopwatch _tpsAverageWatch = Stopwatch.StartNew();
    private void UpdateTpsAverage(int tps) {
        _tpsHistory.Add(tps);

        if (_tpsAverageWatch.ElapsedMilliseconds < AverageLengthMs) return;

        TpsAverage = _tpsHistory.Average();
        TpsAverageString = $"{TpsAverage:N2}";

        if (_tpsAverageWatch.ElapsedMilliseconds < MaxHistorySize) _tpsHistory.Clear();
        _tpsAverageWatch.Restart();
    }
    #endregion
    #endregion
}
