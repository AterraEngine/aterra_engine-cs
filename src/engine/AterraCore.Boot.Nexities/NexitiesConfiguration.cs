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
using static AterraCore.Common.Data.ConfigurationWarnings;
using static Extensions.ServiceDescriptorExtension;

namespace AterraCore.Boot.Nexities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class NexitiesConfiguration(ILogger logger, EngineConfigXml engineConfigDto) : INexitiesConfiguration {
    public LinkedList<ServiceDescriptor> ServicesDefault { get; } = new([
        NewServiceDescriptor<IWorld, World>(ServiceLifetime.Singleton),
    ]);
    
    public LinkedList<ServiceDescriptor> ServicesStatic { get; } = new([
        NewServiceDescriptor<INamedValueConverter, NamedValueConverter>(ServiceLifetime.Singleton),
        NewServiceDescriptor<IAssetAtlas, AssetAtlas>(ServiceLifetime.Singleton),
        NewServiceDescriptor<IAssetInstanceAtlas, AssetInstanceAtlas>(ServiceLifetime.Singleton),
        NewServiceDescriptor<IAssetDataXmlService, AssetDataXmlService>(ServiceLifetime.Singleton)
    ]);

    public EngineConfigXml EngineConfig { get; set; } = engineConfigDto;

    public ConfigurationWarnings Warnings { get; private set; } = Nominal;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
}
