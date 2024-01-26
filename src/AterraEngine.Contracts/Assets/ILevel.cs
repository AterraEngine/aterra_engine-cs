// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Atlases;
using AterraEngine.Types;
using Raylib_cs;

namespace AterraEngine.Contracts.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ILevel : IAsset, IDrawableComponent {
    public List<IActor> Actors { get; }
    public Color BufferBackground { get; }

    public void Unload();
    public void Load(EngineAssetId player2DId, out IPlayer2D player2D);
}