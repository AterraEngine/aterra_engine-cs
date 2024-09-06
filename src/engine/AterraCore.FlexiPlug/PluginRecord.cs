// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Attributes;
using AterraCore.Common.ConfigFiles;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Boot.Logic.PluginLoading.Dto;
using AterraCore.Contracts.FlexiPlug.Plugin;
using AterraCore.Contracts.OmniVault.Assets;
using System.Reflection;

namespace AterraCore.FlexiPlug;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginRecord : IPluginRecord {
    public PluginId PluginId { get; init; }
    public required IEnumerable<Type> Types { get; init; }// DON'T invalidate this !!!

    private Dictionary<Type, AssetTypeRecord>? _assetTypeRecords;
    public IEnumerable<AssetTypeRecord> AssetTypes => (
        _assetTypeRecords ??= Types
            .Where(t =>
                typeof(IAssetInstance).IsAssignableFrom(t)
                && t is { IsInterface: false, IsAbstract: false }
            )
            .Select(t => new { Type = t, AssetAttibute = t.GetCustomAttribute<AssetAttribute>(false) })
            .Where(box => box.AssetAttibute != null)
            .Select(box => new AssetTypeRecord(
                box.Type,
                box.AssetAttibute!,// We check in the where LINQ
                box.Type.GetCustomAttributes<OverridesAssetIdAttribute>(),
                box.Type.GetCustomAttributes<AssetTagAttribute>()
            ))
            .ToDictionary(keySelector: record => record.Type, elementSelector: record => record)
    ).Values;

    public required IPluginBootDto PluginBootDto { get; init; }
    public required PluginConfigXml PluginConfigXml { get; init; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void InvalidateCaches() {
        _assetTypeRecords = null;
    }
}
