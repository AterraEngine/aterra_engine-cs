// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Boot.FlexiPlug;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.FlexiPlug.Plugin;
using CodeOfChaos.Extensions;
using JetBrains.Annotations;
using Serilog;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.FlexiPlug;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class PluginAtlas(ILogger logger) : IPluginAtlas {
    private ILogger Logger => logger.ForContext<PluginAtlas>();
    
    private IReadOnlyDictionary<string, IPluginRecord>? _pluginsByReadableNamesCache;

    private int? _totalAssetCountCache;
    private LinkedList<IPluginRecord> Plugins { get; } = [];
    private IReadOnlyDictionary<string, IPluginRecord> PluginsByReadableNames => _pluginsByReadableNamesCache ??= Plugins
        .Select(p => (ReadableName: p.NameReadable, p))
        .ToDictionary().AsReadOnly();
    
    public int TotalAssetCount => _totalAssetCountCache ??= Plugins.SelectMany(p => p.AssetTypes).Count();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void ImportLoadedPluginDtos(Span<IPreLoadedPluginDto> plugins) {
        foreach (IPreLoadedPluginDto plugin in plugins) {
            Plugins.AddLast(new PluginRecord {
                NameSpace = plugin.ConfigXml.NameSpace,
                NameReadable = plugin.ConfigXml.NameReadable,
                Types = plugin.Types
            });
        }
    }
    public void InvalidateAllCaches() => Plugins.IterateOver(plugin => plugin.InvalidateCaches());

   public IEnumerable<AssetRegistration> GetAssetRegistrations(string? pluginNameSpace = null, CoreTags filter = CoreTags.Asset) {
        // Given this is only done once (during project startup), caching this seems a bit unnecessary.
        return Plugins
            // Filter down to only the plugin we need
            .ConditionalWhere(pluginNameSpace != null, p => p.NameSpace == pluginNameSpace)
            .SelectMany(p => p.AssetTypes
                // Filter down to which Asset Tag we want
                .Where(record => record.AssetAttribute.CoreTags.HasFlag(filter))
                .Select(record => new AssetRegistration {
                    AssetId = record.AssetAttribute.AssetId,
                    InterfaceTypes = record.AssetAttribute.InterfaceTypes,
                    CoreTags = record.AssetAttribute.CoreTags,
                    Type = record.Type,
                    StringTags = record.AssetTagAttributes.SelectMany(attribute => attribute.Tags),
                    OverridableAssetIds = record.OverwritesAssetIdAttributes.Select(attribute => attribute.AssetId),
                })
            );
    }

    public IEnumerable<AssetRegistration> GetEntityRegistrations(string? pluginNameSpace = null) => 
        GetAssetRegistrations(pluginNameSpace, CoreTags.Entity);

    public IEnumerable<AssetRegistration> GetComponentRegistrations(string? pluginNameSpace = null) =>
        GetAssetRegistrations(pluginNameSpace, CoreTags.Component);

    public bool TryGetPluginByReadableName(string readableName, [NotNullWhen(true)] out IPluginRecord? plugin) => 
        PluginsByReadableNames.TryGetValue(readableName, out plugin);
}
