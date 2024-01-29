// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DTO.Components;
using AterraEngine.Types;

namespace AterraEngine.OldContracts.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public delegate T KeyAction<T>(T dto, bool[] args) where T : Input2DDto, new();

public interface IInputComponent<T> : IComponent  where T : Input2DDto, new() {
    public Dictionary<KeyboardInput, KeyAction<T>> KeyMapping { get; }
    
    public T ProcessInput();

}