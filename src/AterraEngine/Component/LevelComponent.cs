// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Interfaces.Component;
namespace AterraEngine.Component;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LevelComponent: ILevelComponent {
    public IDrawableComponent[] DrawableComponents { get; set; } = new IDrawableComponent[0];

    public void Draw() {
        foreach (var t in DrawableComponents) {
            t.Draw();
        }
    }
    
    public void DrawDebug() {
        foreach (var t in DrawableComponents) {
            t.DrawDebug();
        }
    }
}