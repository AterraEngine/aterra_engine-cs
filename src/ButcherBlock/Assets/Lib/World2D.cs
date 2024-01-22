// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine_lib.structs;
using AterraEngine.Interfaces.Assets.Lib;

namespace AterraEngine.Assets.Lib;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class World2D : IWorld2D {
    public ILevel2D Level2D { get; set; }
    public EngineAssetId PlayerId { get; set; }

    public void Draw(Vector2 worldToScreenSpace) {
        Level2D.Draw(worldToScreenSpace);
    }

    public void DrawDebug(Vector2 worldToScreenSpace) {
        Level2D.DrawDebug(worldToScreenSpace);
    }
}