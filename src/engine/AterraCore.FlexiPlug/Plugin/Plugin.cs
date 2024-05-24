// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using AterraCore.Common.FlexiPlug;
using AterraCore.Contracts.FlexiPlug.Plugin;
using AterraCore.Contracts.Nexities.Assets;
namespace AterraCore.FlexiPlug.Plugin;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class Plugin : IPlugin {

    private Dictionary<Type, AssetTypeRecord>? _assetTypeRecords;
    public PluginId Id { get; init; }
    public string ReadableName { get; init; } = "UNDEFINED";
    public List<Assembly> Assemblies { get; init; } = [];

    public IEnumerable<Type> Types { get; init; } = []; // DON'T invalidate this !!!
    public IEnumerable<AssetTypeRecord> AssetTypes {
        get {
            return (_assetTypeRecords ??= Types
                    .Where(t =>
                        typeof(IAssetInstance).IsAssignableFrom(t)
                        && t is { IsInterface: false, IsAbstract: false }
                    )
                    .Select(t => new { Type = t, AssetAttibute = t.GetCustomAttribute<AbstractAssetAttribute>(false) })
                    .Where(box => box.AssetAttibute != null)
                    .Select(box => new AssetTypeRecord(
                        box.Type,
                        box.AssetAttibute!, // We check in the where LINQ
                        box.Type.GetCustomAttributes<AbstractAssetTagAttribute>()
                    ))
                    .ToDictionary(record => record.Type, record => record)
                ).Values;
        }
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void InvalidateCaches() {
        _assetTypeRecords = null;
    }
}