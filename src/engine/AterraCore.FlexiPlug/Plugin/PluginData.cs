// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using AterraCore.Common;
using AterraCore.Contracts.Config.PluginConfig;
using AterraCore.Contracts.FlexiPlug.Plugin;
using AterraCore.Contracts.Nexities.Assets;
using AterraCore.FlexiPlug.Attributes;

namespace AterraCore.FlexiPlug.Plugin;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginData(int id, string filepath) : IPluginData {
    public PluginId Id { get; } = new(id);
    public string FilePath { get; } = filepath;
    public string ReadableName => Data?.ReadableName ?? FilePath;
    public bool IsProcessed { get; set; }

    public IPluginConfigDto<ISemanticVersion>? Data { get; set; }

    private PluginValidity _validity = PluginValidity.Untested;
    public PluginValidity Validity {
        get => _validity;
        set => _validity = _validity != PluginValidity.Invalid ?  value : _validity ; // Once invalid, always invalid
    }
    
    public List<Assembly> Assemblies { get; } = [];
    private IEnumerable<Type>? _types;
    public IEnumerable<Type> Types {
        get { return _types ??= Assemblies.SelectMany(assembly => assembly.GetTypes()); }
    }

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

    public IEnumerable<ServiceData> GetServices() {
        return Types
            .Select(t => new { Type = t, Attribute = t.GetCustomAttribute<ServiceAttribute>() }) // this way we only get the attribute once
            .Where(t => t.Attribute != null)
            .Select(t => new ServiceData(t.Attribute?.Interface!, t.Type));
    }

    public void IngestFromPluginConfigDto(IPluginConfigDto<ISemanticVersion> pluginConfigDto) {
        Data = pluginConfigDto;
    }
}