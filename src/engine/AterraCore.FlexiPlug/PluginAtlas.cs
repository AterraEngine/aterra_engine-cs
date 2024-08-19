// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Boot.Logic.PluginLoading;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.FlexiPlug.Plugin;
using CodeOfChaos.Extensions;
using JetBrains.Annotations;
using Serilog;

namespace AterraCore.FlexiPlug;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class PluginAtlas(ILogger logger) : IPluginAtlas {
    private ILogger Logger => logger.ForContext<PluginAtlas>();

    private int? _totalAssetCountCache;
    private LinkedList<IPluginRecord> Plugins { get; } = [];
    
    public int TotalAssetCount => _totalAssetCountCache ??= Plugins.SelectMany(p => p.AssetTypes).Count();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void ImportLoadedPluginDtos(Span<IPluginDto> plugins) {
        foreach (IPluginDto plugin in plugins) {
            Plugins.AddLast(new PluginRecord {
                PluginId = plugin.PluginId,
                Types = plugin.Types
            });
        }
    }
    public void InvalidateAllCaches() => Plugins.IterateOver(plugin => plugin.InvalidateCaches());

    #region Get Registrations
    public IEnumerable<AssetRegistration> GetAssetRegistrations(PluginId? pluginNameSpace = null, CoreTags? filter = null) {
        // Given this is only done once (during project startup), caching this seems a bit unnecessary.
        return Plugins
            // Filter down to only the plugin we need
            .ConditionalWhere(pluginNameSpace != null, p => p.PluginId == (PluginId)pluginNameSpace!)
            .SelectMany(p => p.AssetTypes
                // Filter down to which Asset Tag we want
                .ConditionalWhere(filter != null, record => record.AssetAttribute.CoreTags.HasFlag(filter!))
                .Select(record => new AssetRegistration(
                    AssetId: record.AssetAttribute.AssetId,
                    Type:record.Type
                    ) {
                    InterfaceTypes = record.AssetAttribute.InterfaceTypes,
                    CoreTags = record.AssetAttribute.CoreTags,
                    StringTags = record.AssetTagAttributes.SelectMany(attribute => attribute.Tags),
                    OverridableAssetIds = record.OverwritesAssetIdAttributes.Select(attribute => attribute.AssetId),
                })
            );
    }

    public IEnumerable<AssetRegistration> GetEntityRegistrations(PluginId? pluginNameSpace = null) => 
        GetAssetRegistrations(pluginNameSpace, CoreTags.Entity);

    public IEnumerable<AssetRegistration> GetComponentRegistrations(PluginId? pluginNameSpace = null) =>
        GetAssetRegistrations(pluginNameSpace, CoreTags.Component);
    #endregion
}
