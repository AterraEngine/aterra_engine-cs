// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.World;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AterraLib.Nexities.Systems.Logic;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System(AssetIdLib.AterraCore.SystemsLogic.CameraController, CoreTags.LogicSystem)]
[Injectable<CameraController>(ServiceLifetime.Singleton)]
[UsedImplicitly]
public class CameraController(ILogger logger): NexitiesSystem<ICamera2D> {
    public override void Tick(ActiveLevel level) {
        foreach (ICamera2D entity in GetEntities(level)) {
            if (Raylib.IsKeyDown(KeyboardKey.Down)) entity.Impulse2D.TranslationOffset = entity.Impulse2D.TranslationOffset with { Y = entity.Impulse2D.TranslationOffset.Y - .5f };
            if (Raylib.IsKeyDown(KeyboardKey.Up)) entity.Impulse2D.TranslationOffset = entity.Impulse2D.TranslationOffset with { Y = entity.Impulse2D.TranslationOffset.Y + .5f };
        
            if (Raylib.IsKeyDown(KeyboardKey.Left)) entity.Impulse2D.TranslationOffset = entity.Impulse2D.TranslationOffset with { X = entity.Impulse2D.TranslationOffset.X - .5f };
            if (Raylib.IsKeyDown(KeyboardKey.Right)) entity.Impulse2D.TranslationOffset = entity.Impulse2D.TranslationOffset with { X = entity.Impulse2D.TranslationOffset.X + .5f };

            if (Raylib.IsKeyDown(KeyboardKey.Z)) entity.Impulse2D.RotationOffset += 1;
            if (Raylib.IsKeyDown(KeyboardKey.X)) entity.Impulse2D.RotationOffset -= 1;

            float scale = 1;
            if(Raylib.IsKeyDown(KeyboardKey.PageUp)) scale += 0.1f;
            if(Raylib.IsKeyDown(KeyboardKey.PageDown)) scale -= 0.1f;
            
            entity.Impulse2D.ScaleOffset = entity.Impulse2D.ScaleOffset with { X = scale};
            entity.Impulse2D.ScaleOffset = entity.Impulse2D.ScaleOffset with { Y = scale};
            
            // TODO do this, but without raylib!
            // while (OS.KeyPresses.TryDequeue(out KeyboardKey key)) {
            //     switch (key) {
            //         case KeyboardKey.Down: {
            //             entity.Impulse2D.TranslationOffset = entity.Impulse2D.TranslationOffset with { Y = entity.Impulse2D.TranslationOffset.Y - .5f };
            //             break;
            //         }
            // }
            //
            // while (Raylib.GetKeyPres; sed() is var key && key != 0) {
            //     logger.Debug("key {key}", key);
            //     
            //     switch (key) {
            //         case (int)KeyboardKey.Down: {
            //             entity.Impulse2D.TranslationOffset = entity.Impulse2D.TranslationOffset with { Y = entity.Impulse2D.TranslationOffset.Y - .5f };
            //             break;
            //         }
            //
            //         case (int)KeyboardKey.Up: {
            //             entity.Impulse2D.TranslationOffset = entity.Impulse2D.TranslationOffset with { Y = entity.Impulse2D.TranslationOffset.Y + .5f };
            //             break;
            //         }
            //
            //         case (int)KeyboardKey.Left: {
            //             entity.Impulse2D.TranslationOffset = entity.Impulse2D.TranslationOffset with { X = entity.Impulse2D.TranslationOffset.X - .5f };
            //             break;
            //         }
            //
            //         case (int)KeyboardKey.Right: {
            //             entity.Impulse2D.TranslationOffset = entity.Impulse2D.TranslationOffset with { X = entity.Impulse2D.TranslationOffset.X + .5f };
            //             break;
            //         }
            //
            //         case (int)KeyboardKey.Z: {
            //             entity.Impulse2D.RotationOffset += 1;
            //             break;
            //         }
            //
            //         case (int)KeyboardKey.X: {
            //             entity.Impulse2D.RotationOffset -= 1;
            //             break;
            //         }
            //
            //         case (int)KeyboardKey.PageUp: {
            //             entity.Impulse2D.ScaleOffset = entity.Impulse2D.ScaleOffset with { X = entity.Impulse2D.ScaleOffset.X + 0.1f};
            //             break;
            //         }
            //
            //         case (int)KeyboardKey.PageDown: {
            //             entity.Impulse2D.ScaleOffset = entity.Impulse2D.ScaleOffset with { Y = entity.Impulse2D.ScaleOffset.Y + 0.1f};
            //             break;
            //         }
            //     }
            // }
        }
    }
}
