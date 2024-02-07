// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using OldAterraEngine.Core.Types.Enums;
using Raylib_cs;

namespace OldAterraEngine.Core.Types;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public struct KeyboardInput(KeyboardKey[] keyboardKeys, KeyboardInputHandling handling) {
    public KeyboardKey[] Keys { get; set; } = keyboardKeys;
    public KeyboardInputHandling Handling { get; } = handling;
}