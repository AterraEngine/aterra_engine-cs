﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.EngineConfig;
using AterraCore.Contracts.Boot.FlexiPlug;
using AterraCore.Contracts.FlexiPlug;
using Microsoft.Extensions.DependencyInjection;

namespace AterraCore.Contracts.Boot;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IBootOperationComponents {
    IWarningAtlas WarningAtlas { get; }
    IFilePathPluginLoader PluginLoader { get; }
    LinkedList<ServiceDescriptor> DefaultServices { get; }
    LinkedList<ServiceDescriptor> StaticServices { get; }
    LinkedList<ServiceDescriptor> DynamicServices { get; }
    LinkedList<IAssemblyLoadedPluginDto> AssemblyLoadedPlugins { get; }
    EngineConfigXml EngineConfigXml { get; set; }
    
    Span<IPluginDto> ValidPlugins { get; }
}
