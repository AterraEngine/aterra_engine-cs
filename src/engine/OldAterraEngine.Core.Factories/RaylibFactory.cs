// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using OldAterraEngine.Contracts.DTOs.EngineConfig;
using OldAterraEngine.Contracts.Factories;
using Raylib_cs;

namespace OldAterraEngine.Core.Factories;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class RaylibFactory: IRaylibFactory {
    public void CreateRaylibWindow(EngineConfigDto engineConfig){
        Raylib.SetConfigFlags(ConfigFlags.Msaa4xHint);
        Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
        
        Raylib.InitWindow(
            engineConfig.RaylibConfig.Window.Screen.Width, 
            engineConfig.RaylibConfig.Window.Screen.Height, 
            engineConfig.RaylibConfig.Window.Title
        );

        if (engineConfig.RaylibConfig.Window.Icon == string.IsNullOrEmpty()) return;
        
        Image iconImage = Raylib.LoadImage(engineConfig.RaylibConfig.Window.Icon);
        Raylib.SetWindowIcon(iconImage);
        Raylib.UnloadImage(iconImage);
    }
}