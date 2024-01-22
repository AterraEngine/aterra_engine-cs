// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Interfaces.Actors;
using AterraEngine.Types;

namespace AterraEngine.Actors;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class Asset(EngineAssetId id, string? internalName) :IAsset {
    public EngineAssetId Id { get; } = id;
    public string? InternalName { get; } = internalName;
    
    public abstract void Draw(Vector2 worldToScreenSpace);
    public abstract void DrawDebug(Vector2 worldToScreenSpace);
}