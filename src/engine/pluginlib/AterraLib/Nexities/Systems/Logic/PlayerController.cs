// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Threading.CrossThread;
using AterraLib.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AterraLib.Nexities.Systems.Logic;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System(AssetIdStringLib.AterraLib.SystemsLogic.PlayerController, CoreTags.LogicThread)]
[Injectable<PlayerController>(ServiceLifetime.Singleton)]
[UsedImplicitly]
public class PlayerController(ICrossThreadTickData crossThreadTickData) : NexitiesSystem<IPlayer2D> {
    public override void Tick(ActiveLevel level) {
        if (!crossThreadTickData.TryGet(AssetTagLib.AterraLib.PlayerInputTickData, out ITickDataInput? playerInputTickData)) return;

        float x = 0f;
        float y = 0f;
        float rotation = 0f;
        Vector2 scale = Vector2.Zero;

        KeyboardKey[] keyMovements = playerInputTickData.KeyboardKeyDown.ToArray();
        for (int i = keyMovements.Length - 1; i >= 0; i--) {
            // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
            switch (keyMovements[i]) {
                case KeyboardKey.W : y -= .25f; break;
                case KeyboardKey.S : y += .25f; break;
                case KeyboardKey.A : x -= .25f; break;
                case KeyboardKey.D : x += .25f; break;
                case KeyboardKey.Q : rotation += .75f; break;
                case KeyboardKey.E : rotation -= .75f; break;
            }
        }

        Vector2[] mouseWheelMovements = playerInputTickData.MouseWheelMovement.ToArray();
        for (int i = mouseWheelMovements.Length - 1; i >= 0; i--) {
            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (mouseWheelMovements[i] ) {
                case { X: var scaleX, Y: 0f }: {
                    scale.Y += scaleX;
                    scale.X += scaleX;
                    break;
                }
                case { X: 0f, Y: var scaleY }: {
                    scale.Y += scaleY;
                    scale.X += scaleY;
                    break;
                }
                case var result: {
                    scale += result;
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
