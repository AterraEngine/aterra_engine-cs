// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Boot.FlexiPlug.PluginLoading;
using AterraCore.Common.ConfigFiles.EngineConfig;
using AterraCore.Common.Data;
using AterraCore.Contracts.Boot;
using AterraCore.Contracts.Boot.FlexiPlug;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.FlexiPlug;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using static Extensions.ServiceDescriptorExtension;

namespace AterraCore.Boot.FlexiPlug;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class FlexiPlugConfiguration(ILogger logger) : IFlexiPlugConfiguration {
    public IEnumerable<ServiceDescriptor> ServicesDefault { get; } = [];
    public IEnumerable<ServiceDescriptor> ServicesStatic { get; } = [
        NewServiceDescriptor<IPluginAtlas, PluginAtlas>(ServiceLifetime.Singleton),
    ];
    
    public IPluginLoader PluginLoader { get; } = new PluginLoader(logger);
    
    private FlexiPlugConfigDto? ConfigDto { get; set; }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public ConfigurationWarnings AsSubConfiguration(IEngineConfiguration engineConfiguration) {
        if (ConfigDto is null) {
            return ConfigurationWarnings.InvalidConfiguration;
        }
        
        // Parse all data from the plugins
        if (!PluginLoader.TryParseAllPlugins(ConfigDto.PluginFilePaths)) {
            logger.Warning("Failed to load all plugins correctly.");
            return ConfigurationWarnings.PluginLoadOrderUnstable | ConfigurationWarnings.UnstablePlugin;
        } 
        
        logger.Information("Plugins successfully loaded.");
        return ConfigurationWarnings.Nominal;
    }
    
    public void ParseDataFromConfig(EngineConfigXml engineConfigDto) {
        ConfigDto = new FlexiPlugConfigDto {
            PluginFilePaths = engineConfigDto.PluginData.LoadOrder.Plugins.Select(
                p => Path.Combine(engineConfigDto.PluginData.RootFolder, p.FilePath)
            )
        };
    }
}
