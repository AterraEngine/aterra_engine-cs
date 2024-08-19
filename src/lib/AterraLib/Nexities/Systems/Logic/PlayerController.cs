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
    public override void Tick(IActiveLevel level) {
        foreach (IPlayer2D entity in GetEntities(level)) {
            
            if (Raylib.IsKeyDown(KeyboardKey.W)) entity.Impulse2D.TranslationOffset = entity.Impulse2D.TranslationOffset with { Y = entity.Impulse2D.TranslationOffset.Y - 10 };
            if (Raylib.IsKeyDown(KeyboardKey.S)) entity.Impulse2D.TranslationOffset = entity.Impulse2D.TranslationOffset with { Y = entity.Impulse2D.TranslationOffset.Y + 10 };
        
            if (Raylib.IsKeyDown(KeyboardKey.A)) entity.Impulse2D.TranslationOffset = entity.Impulse2D.TranslationOffset with { X = entity.Impulse2D.TranslationOffset.X - 10 };
            if (Raylib.IsKeyDown(KeyboardKey.D)) entity.Impulse2D.TranslationOffset = entity.Impulse2D.TranslationOffset with { X = entity.Impulse2D.TranslationOffset.X + 10 };

            if (Raylib.IsKeyDown(KeyboardKey.Q)) entity.Impulse2D.RotationOffset += 1;
            if (Raylib.IsKeyDown(KeyboardKey.E)) entity.Impulse2D.RotationOffset -= 1;

            entity.Impulse2D.ScaleOffset = entity.Impulse2D.ScaleOffset with { X = Raylib.GetMouseWheelMove() * 100 };
            entity.Impulse2D.ScaleOffset = entity.Impulse2D.ScaleOffset with { Y = Raylib.GetMouseWheelMove() * 100 };
        }
    }
}
