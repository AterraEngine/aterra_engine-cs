﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles;
using AterraCore.Common.Types.Nexities;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace AterraCore.Contracts.Boot.Logic.PluginLoading.Dto;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IPluginBootDto {
    string FilePath { get; }
    IReadOnlyCollection<Assembly> Assemblies { get; }
    IReadOnlyCollection<Type> Types { get; }
    PluginId PluginNameSpaceId { get; }
    bool IsValid { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    void SetInvalid();
    bool TryGetPluginConfig([NotNullWhen(true)] out PluginConfigXml? pluginConfigXml);
    bool TrySetPluginConfig(PluginConfigXml pluginConfigXml);
    void UpdateAssemblies(IEnumerable<Assembly> assemblies);
    IEnumerable<(Type Type, T Attribute)> GetOfAttribute<T>() where T : Attribute;
}
