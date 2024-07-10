// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.EngineConfig;
using AterraCore.Contracts.Boot.FlexiPlug;
using AterraCore.Contracts.FlexiPlug;
using Microsoft.Extensions.DependencyInjection;

namespace AterraCore.Boot;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record BootOperationComponents(
    IWarningAtlas WarningAtlas,
    IFilePathPluginLoader PluginLoader
) : IBootOperationComponents {
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
