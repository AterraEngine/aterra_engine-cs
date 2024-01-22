// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Types;

namespace AterraEngine.Interfaces.Component;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IPlayerComponent: IActor2DComponent {
    public Dictionary<KeyboardInput, Action> KeyMapping { get; set; }
    public void LoadKeyMapping();
    public float DeltaTime { get; }
    
}