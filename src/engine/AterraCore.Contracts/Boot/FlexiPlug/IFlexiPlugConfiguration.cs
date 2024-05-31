// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Data;
using AterraCore.Contracts.Boot.Nexities;
using AterraCore.Contracts.FlexiPlug;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AterraCore.Contracts.Boot.FlexiPlug;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IFlexiPlugConfiguration : 
    IHasConfigFactory<IFlexiPlugConfigDto>,
    IHasServices
{
    public IEnumerable<ILoadedPluginDto> LoadedPluginDtos { get; }
    public ConfigurationWarnings Warnings { get; set; }
    public IPluginLoader PluginLoader { get; }
    public ILogger Logger { get; }
    public IEnumerable<ServiceDescriptor> ServiceDescriptors { get; set; }
    
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------

}
