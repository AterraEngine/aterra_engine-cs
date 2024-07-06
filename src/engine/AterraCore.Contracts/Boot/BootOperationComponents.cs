// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.EngineConfig;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace AterraCore.Contracts.Boot;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record BootOperationComponents(
    IConfigurationWarningAtlas WarningAtlas
) {
    public LinkedList<ServiceDescriptor> DefaultServices { get; } = [];
    public LinkedList<ServiceDescriptor> StaticServices { get; } = [];
    
    private EngineConfigXml? _engineConfigXml;
    public EngineConfigXml EngineConfigXml {
        get => _engineConfigXml!;
        set => _engineConfigXml ??= value;
    }
}
