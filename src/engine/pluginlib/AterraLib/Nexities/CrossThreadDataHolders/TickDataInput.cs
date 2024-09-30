// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.AssetVault;
using AterraCore.Common.Attributes.AssetVault;
using AterraCore.Contracts.Threading.CrossData;
using AterraLib.Contracts;
using System.Collections.Concurrent;

namespace AterraLib.Nexities.CrossThreadDataHolders;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[CrossThreadDataHolder<ITickDataInput>(StringAssetIdLib.AterraLib.CrossThreadDataHolders.TickDataInput)]
public class TickDataInput : AssetInstance, IHasLogicTickCleanup, ITickDataInput {
    public ConcurrentStack<KeyboardKey> KeyboardKeyPressed { get; } = [];
    public ConcurrentStack<KeyboardKey> KeyboardKeyPressedRepeated { get; } = [];
    public ConcurrentStack<KeyboardKey> KeyboardKeyReleased { get; } = [];
    public ConcurrentStack<KeyboardKey> KeyboardKeyDown { get; } = [];
    public ConcurrentStack<MouseButton> MouseButtonDown { get; } = [];
    public ConcurrentStack<Vector2> MouseWheelMovement { get; } = [];

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

    public void OnLogicTickCleanup() => ClearCaches();
}
