﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Boot.FlexiPlug.PluginLoading;
using AterraCore.Common.ConfigFiles.EngineConfig;
using AterraCore.Common.Data;
using AterraCore.Contracts.Boot;
using AterraCore.Contracts.Boot.FlexiPlug;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.FlexiPlug;
using Extensions;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Reflection;
using static AterraCore.Common.Data.ConfigurationWarnings;
using static Extensions.ServiceDescriptorExtension;

namespace AterraCore.Boot.FlexiPlug;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class FlexiPlugConfiguration(ILogger logger, EngineConfigXml engineConfigDto) : IFlexiPlugConfiguration {
    public LinkedList<ServiceDescriptor> ServicesDefault { get; } = [];
    public LinkedList<ServiceDescriptor> ServicesStatic { get; } = new ([
        NewServiceDescriptor<IPluginAtlas, PluginAtlas>(ServiceLifetime.Singleton),
    ]);
    
    public IPluginLoader PluginLoader { get; } = new PluginLoader(logger);
    public EngineConfigXml EngineConfig { get; set; } = engineConfigDto;

    public ConfigurationWarnings Warnings { get; private set; } = Nominal;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IFlexiPlugConfiguration CheckAndIncludeRootAssembly() {
        // Include root assembly as the primary plugin
        if (EngineConfig.LoadOrder is not { IncludeRootAssembly: true, RootAssembly: not null }) return this;
        
        PluginLoader.InjectAssemblyAsPlugin(
            Assembly.GetEntryAssembly()!, 
            EngineConfig.LoadOrder.RootAssembly.Author, 
            EngineConfig.LoadOrder.RootAssembly.Name
        );
        logger.Information("Assigned Root Assembly as plugin");
        return this;
    }

    public IFlexiPlugConfiguration PreLoadPlugins() {
        IEnumerable<string> filePaths = EngineConfig.LoadOrder.Plugins.Select(
            p => Path.Combine(EngineConfig.LoadOrder.RootFolderRelative, p.FilePath)
        );
        
        if (PluginLoader.TryParseAllPlugins(filePaths)) return this;
        
        logger.Warning("Failed to load all plugins correctly.");
        Warnings |= PluginLoadOrderUnstable | UnstablePlugin;

        return this;
    }
    
    public ConfigurationWarnings AsSubConfiguration(IEngineConfiguration engineConfiguration) {
        // Assign custom services
        ServicesDefault.AddLastRepeated(PluginLoader.Plugins.SelectMany(dto => dto.GetServicesDefault()));
        ServicesStatic.AddLastRepeated(PluginLoader.Plugins.SelectMany(dto => dto.GetServicesDefault()));
        
        logger.Information("Plugins successfully loaded.");
        return Nominal;
    }
}