// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Assets;
using AterraEngine.Core.Types;

namespace AterraEngine.Contracts.WorldSpaces;

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