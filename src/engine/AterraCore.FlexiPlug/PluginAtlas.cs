// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using AterraCore.Common.FlexiPlug;
using AterraCore.Common.Nexities;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.FlexiPlug.Plugin;
using AterraCore.Contracts.Nexities.Assets;
using Serilog;

namespace AterraCore.FlexiPlug;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class PluginAtlas(ILogger logger) : IPluginAtlas {
    public LinkedList<IPlugin> Plugins { get; private set; } = [];

    // -----------------------------------------------------------------------------------------------------------------
    // Constructor or population Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void ImportPlugins(LinkedList<IPlugin> plugins) => Plugins = plugins;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEnumerable<AssetRegistration> GetAssetRegistrations(int? pluginId=null, CoreTags filter = CoreTags.Asset) {
        return Plugins
            // Filter down to only the plugin we need
            .Where(p => pluginId == null || p.Id == new PluginId((int)pluginId))
            .Select(p => new {PluginId=p.Id, Pairs=p.AssetTypes})
            .SelectMany(o => o.Pairs.SelectMany(
                    pair => pair.Value
                        // Filter down to which Asset Tag we want
                        .Where(a => a.CoreTags.HasFlag(filter))
                        .Select( a => new AssetRegistration {
                            PluginId = o.PluginId,
                            PartialAssetId = a.PartialAssetId,
                            InstanceType = a.InstanceType,
                            CoreTags = a.CoreTags,
                            Type = a.GetType()
                        })
                )
            );
    }

    public IEnumerable<AssetRegistration> GetEntityRegistrations(int? pluginId=null) {
        return GetAssetRegistrations(pluginId, CoreTags.Entity);
    }

    public IEnumerable<AssetRegistration> GetComponentRegistrations(int? pluginId=null) {
        return GetAssetRegistrations(pluginId, CoreTags.Component);
    }
}