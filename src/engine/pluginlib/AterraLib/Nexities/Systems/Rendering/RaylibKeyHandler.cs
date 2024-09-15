// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes;
using AterraCore.Contracts.OmniVault.World;
using AterraCore.Contracts.Threading.CrossThread;
using AterraLib.Nexities.Systems.CrossThreadDataHolders;

namespace AterraLib.Nexities.Systems.Rendering;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[System(StringAssetIdLib.AterraLib.SystemsRendering.RaylibKeyHandler, CoreTags.RenderThread)]
[UsedImplicitly]
public class RaylibKeyHandler(ICrossThreadTickData crossThreadTickData) : NexitiesSystem {
    private static readonly KeyboardKey[] KeyboardKeys = Enum.GetValues<KeyboardKey>();
    private static readonly MouseButton[] MouseButtons = Enum.GetValues<MouseButton>();

    public override void Tick(ActiveLevel level) {
        if (!crossThreadTickData.TryGetOrRegister(AssetTagLib.AterraLib.PlayerInputTickData, out TickDataInput? playerInputTickData)) return;

        for (int i = KeyboardKeys.Length - 1; i >= 0; i--) {
            KeyboardKey key = KeyboardKeys[i];
            if (Raylib.IsKeyPressed(key)) playerInputTickData.KeyboardKeyPressed.Push(key);
            if (Raylib.IsKeyPressedRepeat(key)) playerInputTickData.KeyboardKeyPressedRepeated.Push(key);
            if (Raylib.IsKeyReleased(key)) playerInputTickData.KeyboardKeyReleased.Push(key);
            if (Raylib.IsKeyDown(key)) playerInputTickData.KeyboardKeyDown.Push(key);
            
        }

        for (int i = MouseButtons.Length - 1; i >= 0; i--) {
            MouseButton button = MouseButtons[i];
            if (Raylib.IsMouseButtonDown(button)) playerInputTickData.MouseButtonDown.Push(button);
        }

        Vector2 mouseWheelMovement = Raylib.GetMouseWheelMoveV();
        playerInputTickData.MouseWheelMovement.Push(mouseWheelMovement);
    }
}
