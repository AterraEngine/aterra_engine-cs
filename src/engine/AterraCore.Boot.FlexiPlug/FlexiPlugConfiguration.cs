// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Boot.FlexiPlug.PluginLoading;
using AterraCore.Common.ConfigFiles.EngineConfig;
using AterraCore.Common.Data;
using AterraCore.Contracts.Boot.FlexiPlug;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.FlexiPlug;
using CodeOfChaos.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Reflection;
using static AterraCore.Common.Data.ConfigurationWarnings;
using static CodeOfChaos.Extensions.DependencyInjection.ServiceDescriptorExtension;

namespace AterraCore.Boot.FlexiPlug;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class FlexiPlugConfiguration(
    ILogger logger,
    EngineConfigXml engineConfigDto, 
    IPluginLoader pluginLoader
) : IFlexiPlugConfiguration {
    public LinkedList<ServiceDescriptor> ServicesDefault { get; } = [];
    public LinkedList<ServiceDescriptor> ServicesStatic { get; } = new ([
        NewServiceDescriptor<IPluginAtlas, PluginAtlas>(ServiceLifetime.Singleton),
    ]);
    public EngineConfigXml EngineConfig { get; set; } = engineConfigDto;

    public ConfigurationWarnings Warnings { get; private set; } = Nominal;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IFlexiPlugConfiguration CheckAndIncludeRootAssembly() {
        // Include root assembly as the primary plugin
        if (EngineConfig.LoadOrder is not { RootAssembly: {} rootAssembly}) return this;
        // if (rootNameSpace.IsNotNullOrEmpty()) return this;
        
        pluginLoader.InjectAssemblyAsPlugin(
            Assembly.GetEntryAssembly()!,
            new InjectableAssemblyData (
                rootAssembly.NameSpace ,
                rootAssembly.NameReadable,
                rootAssembly.Author
            )
        );
        logger.Information("Assigned Root Assembly as plugin");
        return this;
    }

    public IFlexiPlugConfiguration PreLoadPlugins() {
        IEnumerable<string> filePaths = EngineConfig.LoadOrder.Plugins.Select(
            p => Path.Combine(EngineConfig.LoadOrder.RootFolderRelative, p.FilePath)
        );

        if (!pluginLoader.TryParseAllPlugins(filePaths)) {
            logger.Warning("Failed to load all plugins correctly.");
            Warnings |= PluginLoadOrderUnstable | UnstablePlugin;

            return this;
        }
        
        ServicesDefault.AddLastRepeated(pluginLoader.Plugins.SelectMany(dto => dto.GetServicesDefault()));
        ServicesStatic.AddLastRepeated(pluginLoader.Plugins.SelectMany(dto => dto.GetServicesStatic()));
        return this;
    }
}
