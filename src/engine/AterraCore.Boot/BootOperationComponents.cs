// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.EngineConfig;
using AterraCore.Contracts.FlexiPlug;
using Microsoft.Extensions.DependencyInjection;

namespace AterraCore.Boot;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record BootOperationComponents(
    IWarningAtlas WarningAtlas
) : IBootOperationComponents {
    public LinkedList<ServiceDescriptor> DefaultServices { get; } = [];
    public LinkedList<ServiceDescriptor> StaticServices { get; } = [];
    public LinkedList<ServiceDescriptor> DynamicServices { get; } = [];
    
    private EngineConfigXml? _engineConfigXml;
    public EngineConfigXml EngineConfigXml {
        get => _engineConfigXml!;
        set => _engineConfigXml ??= value;
    }
    
    private IPluginLoader? _pluginLoader;
    public IPluginLoader PluginLoader {
        get => _pluginLoader!;
        set => _pluginLoader ??= value;
    }
}
