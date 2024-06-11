// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.EngineConfig;
using AterraCore.Common.Data;
using Microsoft.Extensions.DependencyInjection;

namespace AterraCore.Contracts.Boot;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IBootConfiguration {
    public LinkedList<ServiceDescriptor> ServicesDefault { get; }
    public LinkedList<ServiceDescriptor> ServicesStatic { get; }
    public EngineConfigXml EngineConfig { get; set; }
    
    public ConfigurationWarnings Warnings { get; }
}
