// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Contracts.FlexiPlug.Config;
using Microsoft.Extensions.DependencyInjection;
namespace AterraCore.Contracts.FlexiPlug.Plugin;

using Common.Types.FlexiPlug;

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