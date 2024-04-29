// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using AterraCore.Common;
using AterraCore.Common.FlexiPlug;
using AterraCore.Contracts.Config.PluginConfig;
using AterraCore.Contracts.FlexiPlug.Plugin;
using AterraCore.Contracts.Nexities.Assets;
using AterraCore.FlexiPlug.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace AterraCore.FlexiPlug.Plugin;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginDto(int id, string filepath) : IPluginDto {
    // IPluginBase
    public PluginId Id { get; } = new(id);
    public string FilePath { get; } = filepath;
    public string ReadableName => Data?.ReadableName ?? FilePath;
    public List<Assembly> Assemblies { get; } = [];
    
    // Other
    public bool IsProcessed { get; set; }
    public IPluginConfigDto? Data { get; set; }
    private PluginValidity _validity = PluginValidity.Untested;
    public PluginValidity Validity {
        get => _validity;
        set => _validity = _validity != PluginValidity.Invalid ?  value : _validity ; // Once invalid, always invalid
    }
    
    private IEnumerable<Type>? _types;
    public IEnumerable<Type> Types {
        get { return _types ??= Assemblies.SelectMany(assembly => assembly.GetTypes()); }
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEnumerable<ServiceDescriptor> GetServices() {
        return Types
            .Select(t => new { Type = t, Attribute = t.GetCustomAttribute<ServiceAttribute>() }) // this way we only get the attribute once
            .Where(t => t.Attribute != null)
            .Select(t => new ServiceDescriptor(
                serviceType:t.Attribute?.Interface!, 
                implementationType:t.Type, 
                lifetime:(ServiceLifetime)t.Attribute?.Lifetime! // WHY THE HELL DOES THIS NEED TO BE CAST???
            ));
    }

    public void IngestFromPluginConfigDto(IPluginConfigDto pluginConfigDto) {
        Data = pluginConfigDto;
    }
}