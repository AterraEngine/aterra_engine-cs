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
    public IEnumerable<ServiceDescriptor> ServicesDefault { get; }
    public IEnumerable<ServiceDescriptor> ServicesStatic { get; }

    public ConfigurationWarnings AsSubConfiguration(IEngineConfiguration engineConfiguration);
    public void ParseDataFromConfig(EngineConfigXml engineConfigDto);
}
