// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine_lib.structs;
using AterraEngine.Interfaces.Assets;
using AterraEngine.Interfaces.Component;

namespace AterraEngine.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class Asset(EngineAssetId id, string? internalName) :IAsset {
    public EngineAssetId Id { get; } = id;
    public string? InternalName { get; } = internalName;
    
    public abstract void Draw();
    public abstract void DrawDebug();
}