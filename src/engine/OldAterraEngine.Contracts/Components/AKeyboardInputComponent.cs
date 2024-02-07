// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using OldAterraEngine.Contracts.DTOs.Components;
using OldAterraEngine.Core.Types;
using OldAterraEngine.Core.Types.Enums;
using Raylib_cs;

namespace OldAterraEngine.Contracts.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class AInputComponent<T>: IInputComponent<T> where T : Input2DDto, new() {
    public Dictionary<KeyboardInput, KeyAction<T>> KeyMapping { get; } = new();
    
    public T ProcessInput() {
        T dto = new T();
        
        foreach ((KeyboardInput keyInput, var action) in KeyMapping) {
            var args = keyInput.Keys.Select(key => (bool)Raylib.IsKeyDown(key)).ToArray();
            
            switch (keyInput.Handling) {
                case KeyboardInputHandling.All:
                    if (!args.All(arg => arg)) continue; 
                    action(dto, args);
                    break;
                
                case KeyboardInputHandling.Any:
                    if (!args.Any(arg => arg)) continue; 
                    action(dto, args);
                    break;

                case KeyboardInputHandling.None: // TODO implement this
                    if (args.Any(arg => arg)) continue; 
                    action(dto, args);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return dto;
    }
}