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
[System(StringAssetIdLib.AterraLib.SystemsLogic.PlayerController)]
[Injectable<PlayerController>(ServiceLifetime.Singleton)]
[UsedImplicitly]
public class PlayerController(ICrossThreadDataAtlas crossThreadDataAtlas) : NexitiesSystem<IPlayer2D>, ILogicSytem {
    public override void Tick(ActiveLevel level) {
        if (!crossThreadDataAtlas.TryGetOrCreate(AssetIdLib.AterraLib.CrossThreadDataHolders.TickDataInput, out ITickDataInput? playerInputTickData)) return;

        float x = 0f;
        float y = 0f;
        float rotation = 0f;
        Vector2 scale = Vector2.One;

        KeyboardKey[] keyMovements = playerInputTickData.KeyboardKeyDown.Distinct().ToArray();
        for (int i = keyMovements.Length - 1; i >= 0; i--) {
            // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
            switch (keyMovements[i]) {
                case KeyboardKey.W:
                    y -= .25f;
                    break;
                case KeyboardKey.S:
                    y += .25f;
                    break;
                case KeyboardKey.A:
                    x -= .25f;
                    break;
                case KeyboardKey.D:
                    x += .25f;
                    break;
                case KeyboardKey.Q:
                    rotation += .75f;
                    break;
                case KeyboardKey.E:
                    rotation -= .75f;
                    break;
            }
        }

        Vector2[] mouseWheelMovements = playerInputTickData.MouseWheelMovement.Distinct().ToArray();
        for (int i = mouseWheelMovements.Length - 1; i >= 0; i--) {
            switch (mouseWheelMovements[i]) {
                case { X: 0f, Y: 0f }: break;

                case { X: var scaleX, Y: 0f }: {
                    scale.Y = 1 + 0.1f * MathF.Sign(scaleX);
                    scale.X = 1 + 0.1f * MathF.Sign(scaleX);
                    break;
                }

                case { X: 0f, Y: var scaleY }: {
                    scale.Y = 1 + 0.1f * MathF.Sign(scaleY);
                    scale.X = 1 + 0.1f * MathF.Sign(scaleY);
                    break;
                }

                case { X: var scaleX, Y: var scaleY }: {
                    scale.Y = 1 + 0.1f * MathF.Sign(scaleY);
                    scale.X = 1 + 0.1f * MathF.Sign(scaleX);
                    break;
                }
            }
        }

        foreach (IPlayer2D entity in GetEntities(level)) {
            entity.Impulse2D.TranslationOffset += new Vector2(x, y);
            entity.Impulse2D.RotationOffset += rotation;
            entity.Impulse2D.ScaleOffset *= scale;
        }
    }
}
