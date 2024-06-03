// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.EngineConfig;
using AterraCore.Common.Data;
using AterraCore.Contracts.Boot;
using AterraCore.Contracts.Boot.Nexities;
using AterraCore.Contracts.Nexities.Data.Assets;
using AterraCore.Contracts.Nexities.Data.Worlds;
using AterraCore.Contracts.Nexities.DataParsing;
using AterraCore.Contracts.Nexities.DataParsing.NamedValues;
using AterraCore.Nexities.Assets;
using AterraCore.Nexities.Parsers;
using AterraCore.Nexities.Parsers.NamedValues;
using AterraCore.Nexities.Worlds;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using static Extensions.ServiceDescriptorExtension;

namespace AterraCore.Boot.Nexities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class NexitiesConfiguration(ILogger logger) : INexitiesConfiguration {
    public IEnumerable<ServiceDescriptor> ServicesDefault { get; } = [
        NewServiceDescriptor<IWorld, World>(ServiceLifetime.Singleton),
    ];
    public IEnumerable<ServiceDescriptor> ServicesStatic { get; } = [
        NewServiceDescriptor<INamedValueConverter, NamedValueConverter>(ServiceLifetime.Singleton),
        NewServiceDescriptor<IAssetAtlas, AssetAtlas>(ServiceLifetime.Singleton),
        NewServiceDescriptor<IAssetInstanceAtlas, AssetInstanceAtlas>(ServiceLifetime.Singleton),
        NewServiceDescriptor<IAssetDataXmlService, AssetDataXmlService>(ServiceLifetime.Singleton)
    ];
    
    private NexitiesConfigDto? ConfigDto { get; set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public ConfigurationWarnings AsSubConfiguration(IEngineConfiguration engineConfiguration) {
        return ConfigurationWarnings.Nominal;
    }

    public void ParseDataFromConfig(EngineConfigXml engineConfigDto) {
        ConfigDto = new NexitiesConfigDto();
    }
}
