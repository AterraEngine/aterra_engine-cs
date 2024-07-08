// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.EngineConfig;
using AterraCore.Contracts.FlexiPlug;
using Microsoft.Extensions.DependencyInjection;

namespace AterraCore.Contracts.Boot;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IBootOperationComponents {
    IWarningAtlas WarningAtlas { get; }
    LinkedList<ServiceDescriptor> DefaultServices { get; }
    LinkedList<ServiceDescriptor> StaticServices { get; }
    LinkedList<ServiceDescriptor> DynamicServices { get; }
    public EngineConfigXml EngineConfigXml { get; set; }
    public IPluginLoader PluginLoader { get; set; }
}
