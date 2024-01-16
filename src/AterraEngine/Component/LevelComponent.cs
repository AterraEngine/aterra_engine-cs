// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Interfaces.Component;
using AterraEngine.Interfaces.Draw;
using Raylib_cs;

namespace AterraEngine.Component;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LevelComponent: ILevelComponent {
    public IActorComponent[] DrawableComponents { get; set; } = new IActorComponent[0];

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

    public void CollideAll() {
        int count = DrawableComponents.Length;
        List<IActorComponent> removeComps = [];

        for (int i = 0; i < count - 1; i++) {
            for (int j = i + 1; j < count; j++) {
                // Check for collision between DrawableComponents[i] and DrawableComponents[j]
                if (Raylib.CheckCollisionRecs(DrawableComponents[i].Box, DrawableComponents[j].Box)) {
                    removeComps.Add(DrawableComponents[i]);
                }
            }
        }

        foreach (var comp in removeComps) {
            DrawableComponents = DrawableComponents.Where(c => c != comp).ToArray();
        }
    }
}