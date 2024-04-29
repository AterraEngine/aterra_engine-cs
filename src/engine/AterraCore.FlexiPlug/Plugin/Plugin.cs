// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using AterraCore.Common;
using AterraCore.Contracts.FlexiPlug.Plugin;
using AterraCore.Contracts.Nexities.Assets;

namespace AterraCore.FlexiPlug.Plugin;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class Plugin : IPlugin {
    public PluginId Id { get; }
    public string ReadableName { get; }
    public List<Assembly> Assemblies { get; }
    
    private IEnumerable<Type>? _types;
    public IEnumerable<Type> Types => _types ??= Assemblies.SelectMany(assembly => assembly.GetTypes());
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEnumerable<Type> GetAssetTypes() {
        return Types
            .Where(t => 
                typeof(IAsset).IsAssignableFrom(t) 
                && t is { IsInterface: false, IsAbstract: false }
        );
    }
}