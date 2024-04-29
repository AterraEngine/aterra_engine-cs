// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.FlexiPlug;

namespace AterraCore.Common.Nexities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public struct AssetRegistration {
    public PluginId PluginId { get; init; }
    
    public PartialAssetId PartialAssetId { get; init; }
    public AssetInstanceType InstanceType { get; init; }
    public CoreTags CoreTags { get; init; }

    public Type Type { get; init; }
}