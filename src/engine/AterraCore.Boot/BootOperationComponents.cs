// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.EngineConfig;
using AterraCore.Contracts.Boot.Logic.PluginLoading;
using AterraCore.Contracts.FlexiPlug;
using Microsoft.Extensions.DependencyInjection;

namespace AterraCore.Boot;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record BootComponents(
    IFilePathPluginLoader PluginLoader
) : IBootComponents {
    public LinkedList<ServiceDescriptor> DefaultServices { get; } = [];
    public LinkedList<ServiceDescriptor> StaticServices { get; } = [];
    public LinkedList<ServiceDescriptor> DynamicServices { get; } = [];
    
    public LinkedList<IAssemblyLoadedPluginDto> AssemblyLoadedPlugins { get; } = [];
    
    private EngineConfigXml? _engineConfigXml;
    public EngineConfigXml EngineConfigXml {
        get => _engineConfigXml!;
        set => _engineConfigXml ??= value;
    }

    public Span<IPluginDto> ValidPlugins => AssemblyLoadedPlugins
        .Concat<IPluginDto>(PluginLoader.GetValidPlugins()).ToArray();
}
