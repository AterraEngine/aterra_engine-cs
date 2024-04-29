// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using AterraCore.Common;
using AterraCore.Common.FlexiPlug;
using AterraCore.Contracts.FlexiPlug.Plugin;
using AterraCore.Contracts.Nexities.Assets;

namespace AterraCore.FlexiPlug.Plugin;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class Plugin : IPlugin {
    public PluginId Id { get; init; }
    public string ReadableName { get; init; }
    public List<Assembly> Assemblies { get; init; }
    
    public IEnumerable<Type> Types { get; init; } // DON'T invalidate this !!!

    private Dictionary<Type, IEnumerable<AbstractAssetAttribute>>? _cacheTypeToAbstractAssetAttribute;
    public IEnumerable<KeyValuePair<Type, IEnumerable<AbstractAssetAttribute>>> AssetTypes {
        get {
            return _cacheTypeToAbstractAssetAttribute ??= Types
                .Where(t =>
                    typeof(IAsset).IsAssignableFrom(t)
                    && t is { IsInterface: false, IsAbstract: false }
                )
                .SelectMany(t => t.GetCustomAttributes<AbstractAssetAttribute>())
                .GroupBy(a => a.GetType())
                .ToDictionary(g => g.Key, g => g.AsEnumerable());
        }
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void InvalidateCaches() {
        _cacheTypeToAbstractAssetAttribute = null;
    }
}