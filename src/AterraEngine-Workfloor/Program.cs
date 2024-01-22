// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine;
using AterraEngine.EngineLoader;

namespace AterraEngine_Workfloor;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

// ReSharper disable once UnusedType.Global
static class Program {
    public static void Main(string[] args) {
        var engineLoader = new EngineLoader<Engine2D>();
        var engine = engineLoader.CreateEngine();
        engine.Run();
    }
}
