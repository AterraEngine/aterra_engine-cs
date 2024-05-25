﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Numerics;
using AterraCore.Common.Nexities;
using Nexities.Lib.Entities.Actor;
using Serilog;
namespace Nexities.Lib.Systems;

using AterraCore.Nexities.Data.Systems;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System("AF00-0000", AssetInstanceType.Singleton, CoreTags.RenderSystem)]
public class Render2D(ILogger logger) : NexitiesSystem<IActor2D> {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void ProcessEntity(IActor2D entity) {

        foreach (IActor2D childEntity in entity.ChildEntities.OfTypeManyReverse<IActor2D>()) {
            Vector2 translation = childEntity.Transform2D.Translation + entity.Transform2D.Translation;
            Vector2 scale = childEntity.Transform2D.Scale + entity.Transform2D.Scale;
            Vector2 rotation = childEntity.Transform2D.Rotation + entity.Transform2D.Rotation;

            // Print(translation, scale, rotation, childEntity.Sprite2D.Guid);
        }


        logger.Warning("Render2D entity {@e} : {rot} {trans} {scale} ",
            entity,
            entity.Transform2D.Rotation,
            entity.Transform2D.Translation,
            entity.Transform2D.Scale
        );
    }
}