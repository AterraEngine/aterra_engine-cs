// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.OldContracts.Assets;
using AterraEngine.Types;
using Raylib_cs;

namespace AterraEngine.OldContracts.WorldSpaces;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IWorldSpace2D {
    float DeltaTime { get; }
    ILevel? LoadedLevel { get; }
    EngineAssetId StartupLevelId { get; set; }
    
    void RunSetup();
    void RunLogic();
    void RenderFrameUi();
    void RenderFrameWorld();
}