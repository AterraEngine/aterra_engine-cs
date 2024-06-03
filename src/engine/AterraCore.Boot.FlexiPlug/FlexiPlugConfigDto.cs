﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Boot.FlexiPlug;

namespace AterraCore.Boot.FlexiPlug;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class FlexiPlugConfigDto : IFlexiPlugConfigDto {
    public bool IncludeRootAssembly { get; init; } = true;
    public IEnumerable<string> PluginFilePaths { get; init; } = [];
    
    public string? RootAssemblyName {get; init; }
    public string? RootAssemblyAuthor {get; init; }
}
