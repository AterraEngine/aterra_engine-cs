// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraLib.Nexities.Components;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<IRaylibCamera2D>(AssetIdLib.AterraLib.Components.RaylibCamera2D)]
[UsedImplicitly]
public class RaylibCamera2D : NexitiesComponent, IRaylibCamera2D {
    public Camera2D Camera { get; set; } = new() {
        Target = new Vector2(0, 0),
        Offset = new Vector2(Raylib.GetScreenWidth() / 2f, Raylib.GetScreenHeight() / 2f),
        Rotation = 0,
        Zoom = 10
    };
}
