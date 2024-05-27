// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.FlexiPlug.Plugin;
namespace AterraCore.Contracts.FlexiPlug;

using Common.Types.Nexities;
using System.Diagnostics.CodeAnalysis;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IPluginAtlas {
    public int TotalAssetCount { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Constructor or population Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void ImportPlugins(LinkedList<IPlugin> plugins);
    public void InvalidateAllCaches();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEnumerable<AssetRegistration> GetAssetRegistrations(int? pluginId = null, CoreTags filter = CoreTags.Asset);
    public IEnumerable<AssetRegistration> GetEntityRegistrations(int? pluginId = null);
    public IEnumerable<AssetRegistration> GetComponentRegistrations(int? pluginId = null);
    bool TryGetPluginByReadableName(string readableName, [NotNullWhen(true)] out IPlugin? plugin);
}