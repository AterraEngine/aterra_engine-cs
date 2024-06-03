// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.ConfigFiles.PluginConfig;
using AterraCore.Common.Types.FlexiPlug;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AterraCore.Contracts.Boot.FlexiPlug;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ILoadedPluginDto {
    public PluginId Id { get; }
    public string ReadableName { get; }
    public List<Assembly> Assemblies { get; }
    public bool IsProcessed { get; set; }
    public PluginValidity Validity { get; set; }
    public string? CheckSum { get; set; }
    public string ReadableId { get; }
    public IEnumerable<Type> Types { get; }
    public string FilePath { get; }
    
    public PluginConfigXml? Data { get; set; }


    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEnumerable<ServiceDescriptor> GetServices();
    public void IngestFromPluginConfig(PluginConfigXml pluginConfigXml);
}
