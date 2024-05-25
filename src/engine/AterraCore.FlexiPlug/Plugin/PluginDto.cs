﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using AterraCore.Common.FlexiPlug;
using AterraCore.Common.Nexities;
using AterraCore.Contracts.FlexiPlug.Config;
using AterraCore.Contracts.FlexiPlug.Plugin;
using AterraCore.FlexiPlug.Attributes;
using Microsoft.Extensions.DependencyInjection;
namespace AterraCore.FlexiPlug.Plugin;

using Nexities.Data.Components;
using Nexities.Data.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginDto(int id, string filepath) : IPluginDto {
    private string? _readableId;

    private IEnumerable<Type>? _types;
    private PluginValidity _validity = PluginValidity.Untested;
    // IPluginBase
    public PluginId Id { get; } = new(id);
    public string FilePath { get; } = filepath;
    public string ReadableName => Data?.ReadableName ?? FilePath;
    public string ReadableId => _readableId ??= $"Plugin.{Id}";
    public List<Assembly> Assemblies { get; } = [];

    // Other
    public bool IsProcessed { get; set; }
    public IPluginConfigDto? Data { get; set; }
    public PluginValidity Validity {
        get => _validity;
        set => _validity = _validity != PluginValidity.Invalid ? value : _validity; // Once invalid, always invalid
    }
    public string? CheckSum { get; set; } = null;
    public IEnumerable<Type> Types => _types ??= Assemblies.SelectMany(assembly => assembly.GetTypes());

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IEnumerable<ServiceDescriptor> GetServices() {
        return Types
            .Select(t => new { Type = t, Attribute = t.GetCustomAttribute<NexitiesSystemAttribute>(false) }) // this way we only get the attribute once
            .Where(t => t.Attribute != null)
            .Select(t => new ServiceDescriptor(
                t.Attribute?.Interface ?? t.Type,
                t.Type,
                (ServiceLifetime)t.Attribute?.Lifetime! // WHY THE HELL DOES THIS NEED TO BE CAST???
            ));
    }

    public IEnumerable<ServiceDescriptor> GetNexitiesComponents() {
        return Types
            .Select(t => new { Type = t, Attribute = t.GetCustomAttribute<ComponentAttribute>(false) }) // this way we only get the attribute once
            .Where(t => t.Attribute != null)
            .Select(t => new ServiceDescriptor(
                t.Attribute?.Interface ?? t.Type,
                t.Type,
                t.Attribute?.InstanceType switch {
                    AssetInstanceType.Singleton => ServiceLifetime.Singleton,
                    AssetInstanceType.Multiple => ServiceLifetime.Transient,
                    // (AssetInstanceType.Pooled) => ServiceLifetime.Pooled
                    _ => ServiceLifetime.Transient
                }
            ));
    }

    public IEnumerable<ServiceDescriptor> GetNexitiesEntities() {
        return Types
            .Select(t => new { Type = t, Attribute = t.GetCustomAttribute<EntityAttribute>(false) }) // this way we only get the attribute once
            .Where(t => t.Attribute != null)
            .Select(t => new ServiceDescriptor(
                t.Attribute?.Interface ?? t.Type,
                t.Type,
                t.Attribute?.InstanceType switch {
                    AssetInstanceType.Singleton => ServiceLifetime.Singleton,
                    AssetInstanceType.Multiple => ServiceLifetime.Transient,
                    _ => ServiceLifetime.Transient
                }
            ));
    }

    public void IngestFromPluginConfigDto(IPluginConfigDto pluginConfigDto) {
        Data = pluginConfigDto;
    }
}