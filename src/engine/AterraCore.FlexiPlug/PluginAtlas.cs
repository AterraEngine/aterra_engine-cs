// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Types.FlexiPlug;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Boot.FlexiPlug;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.FlexiPlug.Plugin;
using CodeOfChaos.Extensions;
using JetBrains.Annotations;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.FlexiPlug;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

[UsedImplicitly]
public class PluginAtlas : IPluginAtlas {

    private IReadOnlyDictionary<string, IPluginRecord>? _pluginsByReadableNamesCache;

    private int? _totalAssetCountCache;
    public LinkedList<IPluginRecord> Plugins { get; private set; } = [];
    public IReadOnlyDictionary<string, IPluginRecord> PluginsByReadableNames => _pluginsByReadableNamesCache ??= Plugins
        .Select(p => (ReadableName: p.NameReadable, p))
        .ToDictionary().AsReadOnly();
    public int TotalAssetCount => _totalAssetCountCache ??= Plugins.SelectMany(p => p.AssetTypes).Count();

    // -----------------------------------------------------------------------------------------------------------------
    // Constructor or population Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void ImportLoadedPluginDtos(IEnumerable<ILoadedPluginDto> plugins) => Plugins = new LinkedList<IPluginRecord>(
    plugins.Select(
        dto => new PluginRecord {
            NameSpace = dto.NameSpace,
            NameReadable = dto.NameReadable,
            Types = dto.Types
        }
    ));
    public void InvalidateAllCaches() => Plugins.IterateOver(plugin => plugin.InvalidateCaches());

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
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
                    ServiceLifetime = record.AssetAttribute.ServiceLifetime,
                    InterfaceTypes = record.AssetAttribute.InterfaceTypes,
                    CoreTags = record.AssetAttribute.CoreTags,
                    Type = record.Type,
                    StringTags = record.AssetTagAttributes.SelectMany(attrib => attrib.Tags)
                })
            );
    }

    public IEnumerable<AssetRegistration> GetEntityRegistrations(string? pluginNameSpace = null) => GetAssetRegistrations(pluginNameSpace, CoreTags.Entity);

    // Todo add the registration of Named Values here as well
    public IEnumerable<AssetRegistration> GetComponentRegistrations(string? pluginNameSpace = null) => GetAssetRegistrations(pluginNameSpace, CoreTags.Component);

    public bool TryGetPluginByReadableName(string readableName, [NotNullWhen(true)] out IPluginRecord? plugin) => PluginsByReadableNames.TryGetValue(readableName, out plugin);
}
