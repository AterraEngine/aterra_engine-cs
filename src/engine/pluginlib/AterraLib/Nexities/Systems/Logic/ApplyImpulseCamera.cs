﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.Nexities;
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.World;

namespace AterraLib.Nexities.Systems.Logic;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System(StringAssetIdLib.AterraLib.SystemsLogic.ApplyImpulseCamera)]
[UsedImplicitly]
public class ApplyImpulseCamera : NexitiesSystem<ICamera2D>, ILogicSytem {
    public override void Tick(ActiveLevel level) {
        foreach (ICamera2D entity in GetEntities(level)) {
            Vector2 scaledTranslationOffset = entity.RaylibCamera2D.Camera.Zoom != 0
                    ? entity.Impulse2D.TranslationOffset / entity.RaylibCamera2D.Camera.Zoom * 10f
                    : entity.Impulse2D.TranslationOffset
                ;

            entity.RaylibCamera2D.Camera = entity.RaylibCamera2D.Camera with {
                Target = entity.RaylibCamera2D.Camera.Target + scaledTranslationOffset,
                Rotation = entity.RaylibCamera2D.Camera.Rotation + entity.Impulse2D.RotationOffset,
                Zoom = entity.RaylibCamera2D.Camera.Zoom * entity.Impulse2D.ScaleOffset.X,
                Offset = entity.RaylibCamera2D.Camera.Offset
            };

            entity.Impulse2D.Clear();
        }
    }
}
