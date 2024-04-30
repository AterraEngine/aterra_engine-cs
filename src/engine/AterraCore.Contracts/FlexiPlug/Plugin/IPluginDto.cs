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
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEnumerable<ServiceDescriptor> GetServices();
    public void IngestFromPluginConfigDto(IPluginConfigDto pluginConfigDto);
}