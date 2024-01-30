// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.DTOs.Components;
using AterraEngine.Core.Types;

namespace AterraEngine.Contracts.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public delegate T KeyAction<T>(T dto, bool[] args) where T : Input2DDto, new();

public interface IInputComponent<T> : IComponent  where T : Input2DDto, new() {
    public Dictionary<KeyboardInput, KeyAction<T>> KeyMapping { get; }
    
    public T ProcessInput();

}