// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.World;
using AterraCore.FlexiPlug.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace AterraLib.Nexities.Systems.Logic;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System(AssetIdLib.AterraCore.SystemsLogic.PlayerController, CoreTags.LogicSystem)]
[Injectable<PlayerController>(ServiceLifetime.Singleton)]
[UsedImplicitly]
public class PlayerController: NexitiesSystem<IPlayer2D> {
    public override void Tick(IAterraCoreWorld world) {
        IPlayer2D[] entities = GetEntities(world);
        foreach (IPlayer2D entity in entities) {
            
            if (Raylib.IsKeyDown(KeyboardKey.W)) entity.Impulse2D.TranslationOffset = entity.Impulse2D.TranslationOffset with { Y = entity.Impulse2D.TranslationOffset.Y - 10 };
            if (Raylib.IsKeyDown(KeyboardKey.S)) entity.Impulse2D.TranslationOffset = entity.Impulse2D.TranslationOffset with { Y = entity.Impulse2D.TranslationOffset.Y + 10 };
        
            if (Raylib.IsKeyDown(KeyboardKey.A)) entity.Impulse2D.TranslationOffset = entity.Impulse2D.TranslationOffset with { X = entity.Impulse2D.TranslationOffset.X - 10 };
            if (Raylib.IsKeyDown(KeyboardKey.D)) entity.Impulse2D.TranslationOffset = entity.Impulse2D.TranslationOffset with { X = entity.Impulse2D.TranslationOffset.X + 10 };

            if (Raylib.IsKeyDown(KeyboardKey.Q)) entity.Impulse2D.RotationOffset += 10;
            if (Raylib.IsKeyDown(KeyboardKey.E)) entity.Impulse2D.RotationOffset -= 10;

            entity.Impulse2D.ScaleOffset = entity.Impulse2D.ScaleOffset with { X = Raylib.GetMouseWheelMove() * 10 };
            entity.Impulse2D.ScaleOffset = entity.Impulse2D.ScaleOffset with { Y = Raylib.GetMouseWheelMove() * 10 };
        }
    }
}
