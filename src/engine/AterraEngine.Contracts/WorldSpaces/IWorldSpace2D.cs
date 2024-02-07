// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Contracts.Assets;
using AterraEngine.Contracts.ECS.EntityCombinations;
using AterraEngine.Core.Types;

namespace AterraEngine.Contracts.WorldSpaces;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IWorldSpace2D {
    float DeltaTime { get; }
    Vector2 WorldToScreenSpace { get; set; }
    Vector2 ScreenToWorldSpace { get; set; }

    ILevel? LoadedLevel { get; }
    EngineAssetId StartupLevelId { get; set; }
    
    void RunSetup();
    void RunLogic(CancellationToken cancellationToken);
    void RenderFrameUi();
    void RenderFrameWorld();

}