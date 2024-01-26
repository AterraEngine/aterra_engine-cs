// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Atlases;
using AterraEngine.Types;
using Raylib_cs;

namespace AterraEngine.Contracts.Actors;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ILevel : IAsset, IDrawableComponent {
    public List<IActor> Actors { get; init; }
    public Color BufferBackground { get; init; }

    public void Unload();
    public void Load(EngineAssetId player2DId, IAssetAtlas assetAtlas, out IPlayer2D player2D);
}