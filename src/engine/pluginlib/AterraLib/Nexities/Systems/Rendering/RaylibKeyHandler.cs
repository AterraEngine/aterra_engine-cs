﻿// ---------------------------------------------------------------------------------------------------------------------
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

        playerInputTickData.KeyboardKeyDown.PushRange(
            KeyboardKeys.Where(key => Raylib.IsKeyDown(key)).ToArray()
        );

        playerInputTickData.MouseButtonDown.PushRange(
            MouseButtons.Where(button => Raylib.IsMouseButtonDown(button)).ToArray()
        );

        playerInputTickData.MouseWheelMovement.Push(
            Raylib.GetMouseWheelMoveV()
        );
    }
}