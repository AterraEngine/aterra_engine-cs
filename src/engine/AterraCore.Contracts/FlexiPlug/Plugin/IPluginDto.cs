// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.FlexiPlug;
using AterraCore.Contracts.Config.PluginConfig;
using Microsoft.Extensions.DependencyInjection;

namespace AterraCore.Contracts.FlexiPlug.Plugin;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IPluginDto : IPluginBase {
    public string FilePath { get; }
    public bool IsProcessed { get; set; }
    public IPluginConfigDto? Data { get; set; }
    public PluginValidity Validity { get; set; }
    public string? CheckSum { get; set; }
    public string ReadableId { get; }
    
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEnumerable<ServiceDescriptor> GetServices();
    public IEnumerable<ServiceDescriptor> GetNexitiesComponents();
    public IEnumerable<ServiceDescriptor> GetNexitiesEntities();
    public void IngestFromPluginConfigDto(IPluginConfigDto pluginConfigDto);
}