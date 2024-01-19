// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine_lib.structs;
using AterraEngine.Interfaces.Assets;

namespace AterraEngine.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Asset(EngineAssetId id, string? internalName) :IAsset {
    public EngineAssetId Id { get; } = id;
    public string? InternalName { get; } = internalName;
}