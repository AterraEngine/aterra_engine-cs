// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine_lib.structs;
using AterraEngine.Interfaces.Assets.Lib;
using AterraEngine.Interfaces.Component;

namespace AterraEngine.Assets.Lib;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class World2D : IWorld2D {
    public ILevel2D Level2D { get; set; }
    public EngineAssetId PlayerId { get; set; }

    public void Draw() {
        Level2D.Draw();
    }

    public void DrawDebug() {
        Level2D.DrawDebug();
    }
}