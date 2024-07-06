// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Boot.FlexiPlug.PluginLoading;
using AterraCore.Common.ConfigFiles.EngineConfig;
using AterraCore.Common.Data;
using AterraCore.Contracts.Boot.Nexities;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.Nexities.Data.Assets;
using AterraCore.Contracts.Nexities.Data.Worlds;
using AterraCore.Nexities.Assets;
using AterraCore.Nexities.Lib.Components.Transform2D;
using AterraCore.Nexities.Worlds;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Reflection;
using static AterraCore.Common.Data.ConfigurationWarnings;
using static CodeOfChaos.Extensions.DependencyInjection.ServiceDescriptorExtension;

namespace AterraCore.Boot.Nexities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class NexitiesConfiguration(ILogger logger, EngineConfigXml engineConfigDto, IPluginLoader pluginLoader) : INexitiesConfiguration {
    public LinkedList<ServiceDescriptor> ServicesDefault { get; } = new([
        NewServiceDescriptor<IWorld, World>(ServiceLifetime.Singleton),
    ]);
    
    public LinkedList<ServiceDescriptor> ServicesStatic { get; } = new([
        NewServiceDescriptor<IAssetAtlas, AssetAtlas>(ServiceLifetime.Singleton),
        NewServiceDescriptor<IAssetInstanceAtlas, AssetInstanceAtlas>(ServiceLifetime.Singleton),
    ]);

    public EngineConfigXml EngineConfig { get; set; } = engineConfigDto;

    public ConfigurationWarnings Warnings { get; private set; } = Nominal;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public INexitiesConfiguration IncludeNexitiesLibAssembly() {
        pluginLoader.InjectAssemblyAsPlugin(
            Assembly.GetAssembly(typeof(Transform2D))!,
            new InjectableAssemblyData (
                "Nexities" ,
                "Nexities Library",
                "Andreas Sas"
            )
        );
        logger.Information("Assigned Nexities Assembly as plugin");
        return this;
    }
}
