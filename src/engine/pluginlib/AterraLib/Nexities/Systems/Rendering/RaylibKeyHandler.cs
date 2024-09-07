// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Threading.CrossThread;
using AterraLib.Nexities.Systems.CrossThreadDataHolders;

namespace AterraLib.Nexities.Systems.Rendering;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System(AssetIdStringLib.AterraLib.SystemsRendering.RaylibKeyHandler, CoreTags.RenderThread)]
[UsedImplicitly]
public class RaylibKeyHandler(ICrossThreadTickData crossThreadTickData) : NexitiesSystem {
    private static readonly KeyboardKey[] KeyboardKeys = Enum.GetValues<KeyboardKey>();
    private static readonly MouseButton[] MouseButtons = Enum.GetValues<MouseButton>();

    public override void Tick(ActiveLevel level) {
        if (!crossThreadTickData.TryGetOrRegister(AssetTagLib.AterraLib.PlayerInputTickData, out TickDataInput? playerInputTickData)) return;

        foreach (KeyboardKey key in KeyboardKeys) {
            if (Raylib.IsKeyPressed(key)) playerInputTickData.KeyboardKeyPressed.Push(key);
            if (Raylib.IsKeyPressedRepeat(key)) playerInputTickData.KeyboardKeyPressedRepeated.Push(key);
            if (Raylib.IsKeyReleased(key)) playerInputTickData.KeyboardKeyReleased.Push(key);
            if (Raylib.IsKeyDown(key)) playerInputTickData.KeyboardKeyDown.Push(key);
        }

        playerInputTickData.MouseButtonDown.PushRange(
            MouseButtons.Where(button => Raylib.IsMouseButtonDown(button)).ToArray()
        );

        Vector2 mouseWheelMovement = Raylib.GetMouseWheelMoveV();
        playerInputTickData.MouseWheelMovement.Push(mouseWheelMovement);
        // if (mouseWheelMovement.X != 0f && mouseWheelMovement.Y != 0f)
        //     playerInputTickData.MouseWheelMovement.Push(mouseWheelMovement);
    }
}
