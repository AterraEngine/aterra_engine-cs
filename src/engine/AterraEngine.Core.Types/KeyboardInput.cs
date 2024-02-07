// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Core.Types.Enums;
using Raylib_cs;

namespace AterraEngine.Core.Types;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public struct KeyboardInput(KeyboardKey[] keyboardKeys, KeyboardInputHandling handling) {
    public KeyboardKey[] Keys { get; set; } = keyboardKeys;
    public KeyboardInputHandling Handling { get; } = handling;
}