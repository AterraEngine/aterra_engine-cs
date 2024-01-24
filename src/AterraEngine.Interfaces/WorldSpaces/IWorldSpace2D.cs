// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Interfaces.Actors;
using AterraEngine.Types;
using Raylib_cs;

namespace AterraEngine.Interfaces.WorldSpaces;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IWorldSpace2D {
    Camera2D Camera { get; set; }
    IPlayer2D Player2D { get; set; }
    public EngineAssetId Player2DId { get; set; }
    float DeltaTime { get; }

    void RunSetup();
    void UpdateFrame();
    void RenderFrameUi();
    void RenderFrameWorld();
}