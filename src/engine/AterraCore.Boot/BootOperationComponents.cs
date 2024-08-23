// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.EngineConfig;
using AterraCore.Contracts.Boot.Logic.PluginLoading;
using AterraCore.Contracts.Boot.Logic.PluginLoading.Dto;
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
    
    public LinkedList<IPluginBootDto> AssemblyLoadedPlugins { get; } = [];
    
    private EngineConfigXml? _engineConfigXml;
    public EngineConfigXml EngineConfigXml {
        get => _engineConfigXml!;
        set => _engineConfigXml ??= value;
    }

    public Span<IPluginBootDto> ValidPlugins => AssemblyLoadedPlugins
        .Concat(PluginLoader.GetValidPlugins().Select(t => t.pluginBootDto))
        .ToArray();
}
