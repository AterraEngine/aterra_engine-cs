// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.World;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Input;

namespace AterraLib.Nexities.Systems.Logic;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System(AssetIdLib.AterraCore.SystemsLogic.PlayerController, CoreTags.LogicSystem)]
[Injectable<PlayerController>(ServiceLifetime.Singleton)]
[UsedImplicitly]
public class PlayerController: NexitiesSystem<IPlayer2D> {
    public override void Tick(ActiveLevel level) {
        float x = 0f;
        float y = 0f;
        float rotation = 0f;

        if (Raylib.IsKeyDown(KeyboardKey.W)) y -= .25f;
        if (Raylib.IsKeyDown(KeyboardKey.S)) y += .25f;
        
        if (Raylib.IsKeyDown(KeyboardKey.A)) x -= .25f;
        if (Raylib.IsKeyDown(KeyboardKey.D)) x += .25f;

        if (Raylib.IsKeyDown(KeyboardKey.Q)) rotation += .75f;
        if (Raylib.IsKeyDown(KeyboardKey.E)) rotation -= .75f;

        float xScale = Raylib.GetMouseWheelMove() * 4;
        float yScale = Raylib.GetMouseWheelMove() * 4;
        
        foreach (IPlayer2D entity in GetEntities(level)) {
            entity.Impulse2D.TranslationOffset += new Vector2(x, y);
            entity.Impulse2D.RotationOffset += rotation;
            entity.Impulse2D.ScaleOffset += new Vector2(xScale, yScale);
        }
    }
}
