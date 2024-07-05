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
    public void ImportLoadedPluginDtos(IEnumerable<ILoadedPluginDto> plugins);
    public void InvalidateAllCaches();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEnumerable<AssetRegistration> GetAssetRegistrations(string? pluginNameSpace = null, CoreTags filter = CoreTags.Asset);
    public IEnumerable<AssetRegistration> GetEntityRegistrations(string? pluginNameSpace = null);
    public IEnumerable<AssetRegistration> GetComponentRegistrations(string? pluginNameSpace = null);
    bool TryGetPluginByReadableName(string readableName, [NotNullWhen(true)] out IPluginRecord? plugin);
}
