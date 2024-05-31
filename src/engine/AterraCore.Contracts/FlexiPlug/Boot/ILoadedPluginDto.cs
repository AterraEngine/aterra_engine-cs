// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Types.FlexiPlug;
using AterraCore.Contracts.FlexiPlug.Config;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AterraCore.Contracts.FlexiPlug.Boot;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ILoadedPluginDto {
    public PluginId Id { get; }
    public string ReadableName { get; }
    public List<Assembly> Assemblies { get; }
    public bool IsProcessed { get; set; }
    public IPluginConfigDto? Data { get; set; }
    public PluginValidity Validity { get; set; }
    public string? CheckSum { get; set; }
    public string ReadableId { get; }


    public IEnumerable<Type> Types { get; }
    public string FilePath { get; }


    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEnumerable<ServiceDescriptor> GetServices();
    public IEnumerable<ServiceDescriptor> GetNexitiesComponents();
    public IEnumerable<ServiceDescriptor> GetNexitiesEntities();
    public void IngestFromPluginConfigDto(IPluginConfigDto pluginConfigDto);
}
