// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.Nexities;
using AterraCore.DI;
using Serilog;
using System.Net.Http.Headers;

namespace AterraLib.Nexities.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Component<ISystemIds>(StringAssetIdLib.AterraLib.Components.SystemIds)]
public class SystemIds : NexitiesComponent, ISystemIds {
    protected virtual List<AssetId> RawAssetIds { get; } = [];
    public IReadOnlyCollection<AssetId> AssetIds => RawAssetIds.AsReadOnly();

    private static ILogger Logger => EngineServices.GetLogger().ForContext<SystemIds>();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryAdd(AssetId assetId) {
        if (RawAssetIds.Contains(assetId)) return false;

        RawAssetIds.Add(assetId);
        return true;
    }

    public void Add(AssetId assetId) {
        if (!TryAdd(assetId)) Logger.Warning("Failed to add asset id {AssetId} to system ids", assetId);
    }

    public void AddRange(IEnumerable<AssetId> assetIds) {
        foreach (AssetId assetId in assetIds) Add(assetId);
    }
}
