// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine_lib.structs;
using AterraEngine.Plugin;
using Ansi;

namespace EnginePlugin_Test;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Plugin : EnginePlugin {
    public override void ManagedInitialize(PluginId idPrefix) {
        base.ManagedInitialize(idPrefix);
        
        Console.WriteLine(AnsiColor.F("hotpink","Hello there form the plugin"));
        Console.WriteLine(AnsiColor.F("cyan","Schumey_ likes cyan"));
        
    }
}