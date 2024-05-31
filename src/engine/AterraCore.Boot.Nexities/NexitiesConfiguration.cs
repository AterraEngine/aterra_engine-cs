// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.EngineConfig;
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
    public INexitiesConfigDto ConfigDto { get; private set; } = null!;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Services
    // -----------------------------------------------------------------------------------------------------------------
    public IEnumerable<ServiceDescriptor> DefineDefaultServices() => [
        NewServiceDescriptor<IWorld, World>(ServiceLifetime.Singleton),
    ];
    
    public IEnumerable<ServiceDescriptor> DefineStaticServices() => [
        NewServiceDescriptor<INamedValueConverter, NamedValueConverter>(ServiceLifetime.Singleton),
        NewServiceDescriptor<IAssetAtlas, AssetAtlas>(ServiceLifetime.Singleton),
        NewServiceDescriptor<IAssetInstanceAtlas, AssetInstanceAtlas>(ServiceLifetime.Singleton),
        NewServiceDescriptor<IAssetDataXmlService, AssetDataXmlService>(ServiceLifetime.Singleton)
    ];
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void StoreDataFromConfig(EngineConfigXml source) => ConfigDto = ExtractDataFromConfig(source);
    public INexitiesConfigDto ExtractDataFromConfig(EngineConfigXml source) {
        return new NexitiesConfigDto();
    }

}
