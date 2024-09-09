﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Threading.CrossThread;
using Raylib_cs;
using System.Collections.Concurrent;
using System.Numerics;

namespace AterraLib.Contracts;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ITickDataInput : ITickDataHolder {
    ConcurrentStack<KeyboardKey> KeyboardKeyPressed { get; }
    ConcurrentStack<KeyboardKey> KeyboardKeyPressedRepeated { get; }
    ConcurrentStack<KeyboardKey> KeyboardKeyReleased { get; }
    ConcurrentStack<KeyboardKey> KeyboardKeyDown { get; }
    ConcurrentStack<MouseButton> MouseButtonDown { get; }
    ConcurrentStack<Vector2> MouseWheelMovement { get; }
}