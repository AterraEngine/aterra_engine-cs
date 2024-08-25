// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;

namespace AterraEngine.Threading.Render;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public partial class RenderThreadProcessor {
    private void OnEventManagerOnEventWindowResized() {
        if (world.ActiveLevel is not { Camera2DEntity: {} camera2DEntity }) return;
        camera2DEntity.Camera = camera2DEntity.Camera with {
            Offset = new Vector2(Raylib.GetScreenWidth() / 2f, Raylib.GetScreenHeight() / 2f)
        };
    }
}
