// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using AterraCore.Common.FlexiPlug;
using AterraCore.Common.Nexities;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.FlexiPlug.Plugin;
using JetBrains.Annotations;
using static Extensions.LinqExtensions;

namespace AterraCore.FlexiPlug;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

[UsedImplicitly]
public class PluginAtlas : IPluginAtlas {
    public LinkedList<IPlugin> Plugins { get; private set; } = [];

    private int? _totalAssetCountCache;
    public int TotalAssetCount => _totalAssetCountCache ??= Plugins.SelectMany(p => p.AssetTypes).Count();
    
    private IReadOnlyDictionary<string, IPlugin>? _pluginsByReadableNamesCache ;
    public IReadOnlyDictionary<string, IPlugin> PluginsByReadableNames => _pluginsByReadableNamesCache ??= Plugins
        .Select(p => (p.ReadableName, p))
        .ToDictionary().AsReadOnly();

    // -----------------------------------------------------------------------------------------------------------------
    // Constructor or population Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void ImportPlugins(LinkedList<IPlugin> plugins) => Plugins = plugins;
    public void InvalidateAllCaches() => Plugins.IterateOver(plugin => plugin.InvalidateCaches());

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEnumerable<AssetRegistration> GetAssetRegistrations(int? pluginId=null, CoreTags filter = CoreTags.Asset) {
        // GIven this is only done once (during project startup), caching this seems a bit unnecessary.
        
        return Plugins
            // Filter down to only the plugin we need
            .Where(p => pluginId == null || p.Id == new PluginId((int)pluginId))
            .Select(p => new {PluginId=p.Id, Pairs=p.AssetTypes})
            .SelectMany(box => box.Pairs
                // Filter down to which Asset Tag we want
                .Where(record => record.AssetAttribute.CoreTags.HasFlag(filter))
                .Select( record => new AssetRegistration {
                    PluginId = box.PluginId,
                    PartialAssetId = record.AssetAttribute.PartialAssetId,
                    InstanceType = record.AssetAttribute.InstanceType,
                    CoreTags = record.AssetAttribute.CoreTags,
                    Type = record.Type,
                    StringTags = record.AssetTagAttributes.SelectMany(attrib => attrib.Tags)
                })
            );
    }

    public IEnumerable<AssetRegistration> GetEntityRegistrations(int? pluginId=null) {
        return GetAssetRegistrations(pluginId, CoreTags.Entity);
    }

    public IEnumerable<AssetRegistration> GetComponentRegistrations(int? pluginId=null) {
        return GetAssetRegistrations(pluginId, CoreTags.Component);
    }

    private bool TryGetPluginByReadableName(string readableName, [NotNullWhen(true)] out IPlugin? plugin) {
        return PluginsByReadableNames.TryGetValue(readableName, out plugin);
    }
}