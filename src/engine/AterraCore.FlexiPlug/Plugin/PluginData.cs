// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using AterraCore.Common;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.FlexiPlug.Plugin;
using AterraCore.Contracts.Nexities.Assets;
using AterraCore.FlexiPlug.Attributes;

namespace AterraCore.FlexiPlug.Plugin;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginData(int id, Assembly assembly) : IPluginData {
    public PluginId Id { get; } = new(id);
    public Assembly Assembly { get; } = assembly;
    private readonly IEnumerable<Type> _types = assembly.GetTypes();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEnumerable<Type> GetAssetTypes() {
        return _types
            .Where(t => 
                typeof(IAsset).IsAssignableFrom(t) 
                && t is { IsInterface: false, IsAbstract: false }
        );
    }

    public IEnumerable<ServiceData> GetServices() {
        return _types
            .Select(t => new { Type = t, Attribute = t.GetCustomAttribute<ServiceAttribute>() }) // this way we only get the attribute once
            .Where(t => t.Attribute != null)
            .Select(t => new ServiceData(t.Attribute?.Interface!, t.Type));
    }
    
}