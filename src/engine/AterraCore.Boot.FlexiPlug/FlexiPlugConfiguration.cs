// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Boot.FlexiPlug.PluginLoading;
using AterraCore.Common.ConfigFiles.EngineConfig;
using AterraCore.Common.Data;
using AterraCore.Contracts.Boot.FlexiPlug;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.Nexities.Data.Assets;
using AterraCore.FlexiPlug;
using AterraCore.Nexities.Assets;
using Extensions;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using static Extensions.ServiceDescriptorExtension;

namespace AterraCore.Boot.FlexiPlug;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class FlexiPlugConfiguration(ILogger logger) : IFlexiPlugConfiguration {
    public IEnumerable<ILoadedPluginDto> LoadedPluginDtos { get; } = [];
    public IFlexiPlugConfigDto ConfigDto { get; private set; } = null!;
    public ConfigurationWarnings Warnings { get; set; } = ConfigurationWarnings.Nominal;
    public IPluginLoader PluginLoader { get; } = new PluginLoader(logger);
    public ILogger Logger { get; } = logger;

    public IEnumerable<ServiceDescriptor> ServiceDescriptors { get; set; } = [];
    
    // -----------------------------------------------------------------------------------------------------------------
    // Services
    // -----------------------------------------------------------------------------------------------------------------
    public IEnumerable<ServiceDescriptor> DefineDefaultServices() => [
        ..ServiceDescriptors
    ];
    public IEnumerable<ServiceDescriptor> DefineStaticServices() => [
        NewServiceDescriptor<IPluginAtlas, PluginAtlas>(ServiceLifetime.Singleton),
    ];

    // -----------------------------------------------------------------------------------------------------------------
    // Config mappers
    // -----------------------------------------------------------------------------------------------------------------
    public void StoreDataFromConfig(EngineConfigXml source) => ConfigDto = ExtractDataFromConfig(source);
    public IFlexiPlugConfigDto ExtractDataFromConfig(EngineConfigXml source) {
        return new FlexiPlugConfigDto {
            PluginFilePaths = source.PluginData.LoadOrder.Plugins.Select(
                p => Path.Combine(source.PluginData.RootFolder, p.FilePath)
            )
        };
    }

}
