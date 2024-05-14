﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using AterraCore.Common;
using AterraCore.Common.Config;
using AterraCore.Contracts.DI;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.FlexiPlug.Plugin;
using AterraCore.FlexiPlug;
using AterraCore.FlexiPlug.Plugin;
using AterraEngine.Config;
using Extensions;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Xml;

namespace AterraEngine.Configuration;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class PluginConfiguration(ILogger logger, IEnumerable<string> filePaths, IEngineServiceBuilder serviceBuilder) {
    private readonly PluginLoader _pluginLoader = new(logger);
    public ConfigurationWarnings ConfigurationWarnings { get; private set; }= ConfigurationWarnings.Nominal;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public PluginConfiguration ImportAssemblies(params BareAssemblyPlugin[] assemblies) {
        foreach (BareAssemblyPlugin assembly in assemblies) {
            _pluginLoader.InjectAssemblyAsPlugin(assembly);
        }
        return this;
    }
    
    public PluginConfiguration ImportPlugins() {
        if(!_pluginLoader.TryParseAllPlugins(filePaths)){
            ConfigurationWarnings |= ConfigurationWarnings.PluginLoadOrderUnstable | ConfigurationWarnings.UnstablePlugin;
            logger.Warning("Failed to load all plugins correctly.");
        } else {
            logger.Information("Plugins successfully loaded.");
        }

        return this;
    }
    public PluginConfiguration AssignServices() {
        // TODO make a check though the _engineConfigFlag to see if everything has been setup already
        
        _pluginLoader.Plugins
            .SelectMany<IPluginDto, ServiceDescriptor>(p => 
                p.GetServices()
                    .Concat(p.GetNexitiesComponents())
                    .Concat(p.GetNexitiesEntities())
            )
            .IterateOver(serviceBuilder.AssignFromServiceDescriptor);
        
        logger.Information("Assigned Systems from Plugins");
        return this;
    }

    public LinkedList<IPlugin> CreatePluginList() {
        return new LinkedList<IPlugin>(
            _pluginLoader.Plugins.Select(p => new Plugin {
                Id = p.Id,
                ReadableName = p.ReadableName,
                Assemblies = p.Assemblies,
                Types = p.Types
            })
        );
    }
}