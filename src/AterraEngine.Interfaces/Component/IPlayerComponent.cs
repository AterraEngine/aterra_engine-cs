// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine_lib.structs;

namespace AterraEngine.Interfaces.Component;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IPlayerComponent: IActor2DComponent {
    public Dictionary<KeyboardInput, Action> KeyMapping { get; set; }
    public void LoadKeyMapping();
    public float DeltaTime { get; }
    
}