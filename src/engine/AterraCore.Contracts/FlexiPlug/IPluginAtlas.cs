// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Boot.FlexiPlug;
using AterraCore.Contracts.FlexiPlug.Plugin;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Contracts.FlexiPlug;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IPluginAtlas {
    public int TotalAssetCount { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Constructor or population Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void ImportLoadedPluginDtos(Span<IPluginDto> plugins);
    public void InvalidateAllCaches();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEnumerable<AssetRegistration> GetAssetRegistrations(PluginId? pluginNameSpace = null, CoreTags? filter = null);
    public IEnumerable<AssetRegistration> GetEntityRegistrations(PluginId? pluginNameSpace = null);
    public IEnumerable<AssetRegistration> GetComponentRegistrations(PluginId? pluginNameSpace = null);
}
