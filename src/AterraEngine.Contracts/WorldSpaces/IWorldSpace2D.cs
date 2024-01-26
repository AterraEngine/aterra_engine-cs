// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Actors;
using AterraEngine.Types;
using Raylib_cs;

namespace AterraEngine.Contracts.WorldSpaces;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IWorldSpace2D {
    Camera2D Camera { get; set; }
    IPlayer2D Player2D { get; set; }
    EngineAssetId Player2DId { get; set; }
    float DeltaTime { get; }
    ILevel? LoadedLevel { get; }
    EngineAssetId StartupLevelId { get; set; }
    
    void RunSetup();
    void UpdateFrame();
    void RenderFrameUi();
    void RenderFrameWorld();
}