// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Boot.Logic.PluginLoading;
using AterraCore.Contracts.Boot.Logic.PluginLoading.Dto;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Contracts.FlexiPlug;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IPluginAtlas {
    public int TotalAssetCount { get; }
    public IReadOnlySet<PluginId> PluginIds { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Constructor or population Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void ImportLoadedPluginDtos(Span<IPluginBootDto> plugins);
    public void InvalidateAllCaches();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEnumerable<AssetRegistration> GetAssetRegistrations(PluginId? pluginNameSpace = null, CoreTags? filter = null);
    public IEnumerable<AssetRegistration> GetEntityRegistrations(PluginId? pluginNameSpace = null);
    public IEnumerable<AssetRegistration> GetComponentRegistrations(PluginId? pluginNameSpace = null);

    public bool TryGetFileRawFromPluginZip(PluginId pluginId, string internalFilePath, [NotNullWhen(true)] out byte[]? bytes);
}
