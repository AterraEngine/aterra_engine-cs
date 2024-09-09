// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Data;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Boot.Logic.PluginLoading.Dto;
using AterraCore.Contracts.FlexiPlug.Plugin;
using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Contracts.FlexiPlug;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IPluginAtlas {
    public int TotalAssetCount { get; }
    public IReadOnlyCollection<IPluginRecord> Plugins { get; init; }
    public FrozenSet<PluginId> PluginIds { get; init; }
    public ImmutableArray<PluginId> PluginIdsByOrder { get; init; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEnumerable<AssetRegistration> GetAssetRegistrations(PluginId? pluginNameSpace = null, CoreTags? filter = null);
    public IEnumerable<AssetRegistration> GetEntityRegistrations(PluginId? pluginNameSpace = null);
    public IEnumerable<AssetRegistration> GetComponentRegistrations(PluginId? pluginNameSpace = null);

    public bool TryGetFileRawFromPluginZip(PluginId pluginId, string internalFilePath, [NotNullWhen(true)] out byte[]? bytes);

    public bool IsLoadedBefore(PluginId left, PluginId right);
    public bool IsLoadedAfter(PluginId left, PluginId right);
}
