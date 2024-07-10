// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.EngineConfig;
using AterraCore.Contracts.Boot.Logic.PluginLoading;
using AterraCore.Contracts.FlexiPlug;
using Microsoft.Extensions.DependencyInjection;

namespace AterraCore.Contracts.Boot;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IBootComponents {
    IFilePathPluginLoader PluginLoader { get; }
    LinkedList<ServiceDescriptor> DefaultServices { get; }
    LinkedList<ServiceDescriptor> StaticServices { get; }
    LinkedList<ServiceDescriptor> DynamicServices { get; }
    LinkedList<IAssemblyLoadedPluginDto> AssemblyLoadedPlugins { get; }
    EngineConfigXml EngineConfigXml { get; set; }
    
    Span<IPluginDto> ValidPlugins { get; }
}
