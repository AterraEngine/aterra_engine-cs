// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.PluginConfig;
using AterraCore.Common.Types.FlexiPlug;
using AterraCore.Contracts.Boot.FlexiPlug;
using AterraCore.FlexiPlug.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AterraCore.Boot.FlexiPlug.PluginLoading;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LoadedPluginDto(int id, string filepath) : ILoadedPluginDto {
    public PluginId Id { get; } = new(id);
    public string FilePath { get; } = filepath;
    public string ReadableName => Data?.ReadableName ?? FilePath;
    
    private string? _readableId;
    public string ReadableId => _readableId ??= $"Plugin.{Id}";
    
    public bool IsProcessed { get; set; }
    public PluginConfigXml? Data { get; set; }

    private PluginValidity _validity = PluginValidity.Untested;
    public PluginValidity Validity {
        get => _validity;
        set => _validity = _validity != PluginValidity.Invalid ? value : _validity;// Once invalid, always invalid
    }

    public string? CheckSum { get; set; } = null;
    
    public List<Assembly> Assemblies { get; } = [];
    private IEnumerable<Type>? _types;
    public IEnumerable<Type> Types => _types ??= Assemblies.SelectMany(assembly => assembly.GetTypes());

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEnumerable<ServiceDescriptor> GetServices() {
        return Types
            .Select(t => new { Type = t, Attribute = t.GetCustomAttribute<NexitiesSystemAttribute>(false) })// this way we only get the attribute once
            .Where(t => t.Attribute != null)
            .Select(t => new ServiceDescriptor(
            t.Attribute?.Interface ?? t.Type,
            t.Type,
            (ServiceLifetime)t.Attribute?.Lifetime!// WHY THE HELL DOES THIS NEED TO BE CAST???
            ));
    }

    public void IngestFromPluginConfig(PluginConfigXml pluginConfigDto) {
        
    }
}
