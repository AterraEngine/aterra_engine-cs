// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Numerics;
using AterraEngine.Interfaces.Draw;
using Raylib_cs;

namespace AterraEngine.Interfaces.Component;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IPlayerComponent: IActorComponent {
    public Dictionary<KeyboardKey, Action> KeyMapping { get; set; }
    public void LoadKeyMapping();
}