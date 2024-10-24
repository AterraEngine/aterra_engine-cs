﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.DI;
using AterraCore.Common.Attributes.Nexities;
using AterraCore.Contracts.Nexities.Systems;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Threading.CrossData;
using AterraLib.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace AterraLib.Nexities.Systems.Logic;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System(StringAssetIdLib.AterraLib.SystemsLogic.CameraController)]
[Injectable<CameraController>(ServiceLifetime.Singleton)]
[UsedImplicitly]
public class CameraController(ICrossThreadDataAtlas crossThreadDataAtlas) : NexitiesSystem<ICamera2D>, ILogicSytem {
    public override void Tick(ActiveLevel level) {
        if (!crossThreadDataAtlas.TryGetOrCreate(AssetIdLib.AterraLib.CrossThreadDataHolders.TickDataInput, out ITickDataInput? playerInputTickData)) return;

        float translationX = 0.0f;
        float translationY = 0.0f;
        float rotationOffset = 0f;
        float scaleX = 1f;

        KeyboardKey[] keyMovements = playerInputTickData.KeyboardKeyDown.ToHashSet().ToArray();
        for (int i = keyMovements.Length - 1; i >= 0; i--) {
            // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
            switch (keyMovements[i]) {
                case KeyboardKey.Up:
                    translationY -= 1f;
                    break;
                case KeyboardKey.Down:
                    translationY += 1f;
                    break;
                case KeyboardKey.Left:
                    translationX -= 1f;
                    break;
                case KeyboardKey.Right:
                    translationX += 1f;
                    break;
                case KeyboardKey.Z:
                    rotationOffset += 1f;
                    break;
                case KeyboardKey.X:
                    rotationOffset -= 1f;
                    break;
                case KeyboardKey.PageUp:
                    scaleX += .1f;
                    break;
                case KeyboardKey.PageDown:
                    scaleX -= .1f;
                    break;
            }
        }

        foreach (ICamera2D entity in GetEntities(level)) {
            entity.Impulse2D.TranslationOffset = entity.Impulse2D.TranslationOffset with {
                X = entity.Impulse2D.TranslationOffset.X + translationX,
                Y = entity.Impulse2D.TranslationOffset.Y + translationY
            };

            entity.Impulse2D.ScaleOffset = entity.Impulse2D.ScaleOffset with {
                X = entity.Impulse2D.ScaleOffset.X * scaleX,
                Y = entity.Impulse2D.ScaleOffset.Y * scaleX
            };

            entity.Impulse2D.RotationOffset += rotationOffset;
        }
    }
}
