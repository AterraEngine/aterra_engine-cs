// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraLib.Contracts;
using System.Collections.Concurrent;

namespace AterraLib.Nexities.Systems.CrossThreadDataHolders;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TickDataInput : ITickDataInput {
    public ConcurrentStack<KeyboardKey> KeyboardKeyPressed { get; } = [];
    public ConcurrentStack<KeyboardKey> KeyboardKeyPressedRepeated { get; } = [];
    public ConcurrentStack<KeyboardKey> KeyboardKeyReleased { get; } = [];
    public ConcurrentStack<KeyboardKey> KeyboardKeyDown { get; } = [];
    public ConcurrentStack<MouseButton> MouseButtonDown { get; } = [];
    public ConcurrentStack<Vector2> MouseWheelMovement { get; } = [];

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Clear() {
        KeyboardKeyPressed.Clear();
        KeyboardKeyPressedRepeated.Clear();
        KeyboardKeyReleased.Clear();
        KeyboardKeyDown.Clear();
        MouseButtonDown.Clear();
        MouseWheelMovement.Clear();
    }

    public bool IsEmpty => KeyboardKeyPressed.IsEmpty
        && KeyboardKeyPressedRepeated.IsEmpty
        && KeyboardKeyReleased.IsEmpty
        && KeyboardKeyDown.IsEmpty
        && MouseButtonDown.IsEmpty
        && MouseWheelMovement.IsEmpty;
}
