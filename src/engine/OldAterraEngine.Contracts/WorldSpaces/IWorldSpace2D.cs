// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using OldAterraEngine.Core.Types;
using OldAterraEngine.Contracts.Assets;

namespace OldAterraEngine.Contracts.WorldSpaces;

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