// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.Entities;
using AterraCore.Contracts.Nexities.Levels;
using AterraCore.FlexiPlug.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace AterraLib.Nexities.Systems.Logic;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System("AterraLib:Nexities/Systems/PlayerController", CoreTags.LogicSystem)]
[Injectable<PlayerController>(ServiceLifetime.Singleton)]
[UsedImplicitly]
public class PlayerController: NexitiesSystem<IPlayer2D> {

    protected override IEnumerable<IPlayer2D> SelectEntities(INexitiesLevel? level) => level?.ChildrenIDs.OfType<IPlayer2D>() ?? [];
    protected override void ProcessEntity(IPlayer2D entity) {
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
