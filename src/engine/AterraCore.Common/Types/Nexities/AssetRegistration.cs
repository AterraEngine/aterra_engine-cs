﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Common.Types.Nexities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public struct AssetRegistration {
    // Generated by PluginLoader
    public AssetId AssetId { get; init; }

    // Actual type of class
    public Type Type { get; init; }
    public Type[] InterfaceTypes { get; init; }

    // Data from IAssetAttribute
    public CoreTags CoreTags { get; init; }

    // Data from IAssetTagAttribute
    public IEnumerable<string> StringTags { get; init; }
    
    // Data from AbstractOverwritesAssetTagAttribute
    public IEnumerable<AssetId> OverridableAssetIds { get; init; } 
}
