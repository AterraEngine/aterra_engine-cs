// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine_lib.structs;

namespace AterraEngine.Interfaces.Component;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IPlayerComponent: IActorComponent {
    public Dictionary<KeyboardInput, Action> KeyMapping { get; set; }
    public void LoadKeyMapping();
    public float DeltaTime { get; }
    
}