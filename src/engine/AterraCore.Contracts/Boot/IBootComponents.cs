// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.EngineConfig;
using AterraCore.Contracts.Boot.Logic.PluginLoading.Dto;
using AterraCore.Contracts.FlexiPlug;
using Microsoft.Extensions.DependencyInjection;

namespace AterraCore.Contracts.Boot;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IBootComponents {
    IServiceCollection Services { get; }
    
    IFilePathPluginLoader PluginLoader { get; }
    LinkedList<ServiceDescriptor> DefaultServices { get; }
    LinkedList<ServiceDescriptor> StaticServices { get; }
    LinkedList<ServiceDescriptor> DynamicServices { get; }
    LinkedList<IPluginBootDto> AssemblyLoadedPlugins { get; }
    EngineConfigXml EngineConfigXml { get; set; }

    Span<IPluginBootDto> ValidPlugins { get; }
}
