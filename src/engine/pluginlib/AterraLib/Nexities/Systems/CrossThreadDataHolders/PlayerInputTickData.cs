// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.Nexities;
using AterraCore.Contracts.Threading.CrossThread;
using AterraCore.OmniVault.Assets;
using AterraLib.Contracts;
using System.Collections.Concurrent;

namespace AterraLib.Nexities.Systems.CrossThreadDataHolders;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[DataHolder(StringAssetIdLib.AterraLib.TickDataHolders.PlayerInputTickData)]
public class PlayerInputTickData : AssetInstance, IPlayerInputTickData, IHasLevelChangeCleanup, IHasLogicTickCleanup {
    public ConcurrentStack<KeyboardKey> KeyboardKeyPressed { get; } = [];
    public ConcurrentStack<KeyboardKey> KeyboardKeyPressedRepeated { get; } = [];
    public ConcurrentStack<KeyboardKey> KeyboardKeyReleased { get; } = [];
    public ConcurrentStack<KeyboardKey> KeyboardKeyDown { get; } = [];
    public ConcurrentStack<MouseButton> MouseButtonDown { get; } = [];
    public ConcurrentStack<Vector2> MouseWheelMovement { get; } = [];

    public void OnLevelChangeCleanup() => ClearCaches();
    public void OnLogicTickCleanup() => ClearCaches();

    public bool IsEmpty => KeyboardKeyPressed.IsEmpty
        && KeyboardKeyPressedRepeated.IsEmpty
        && KeyboardKeyReleased.IsEmpty
        && KeyboardKeyDown.IsEmpty
        && MouseButtonDown.IsEmpty
        && MouseWheelMovement.IsEmpty;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private void ClearCaches() {
        KeyboardKeyPressed.Clear();
        KeyboardKeyPressedRepeated.Clear();
        KeyboardKeyReleased.Clear();
        KeyboardKeyDown.Clear();
        MouseButtonDown.Clear();
        MouseWheelMovement.Clear();
    }
}
