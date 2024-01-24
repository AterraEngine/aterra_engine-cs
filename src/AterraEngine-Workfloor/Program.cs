// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using ArgsParser;
using AterraEngine;
using AterraEngine.EngineLoader;

namespace AterraEngine_Workfloor;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ArgOptions {
    [ArgFlag('h', "help")]      public bool ShowHelp { get; set; } = false;
    [ArgFlag('v', "version")]   public bool ShowVersion { get; set; } = false;
    [ArgFlag('d', "debug")]     public bool EnableDebug { get; set; } = false;
    
    [ArgFlag('r', "raylib")]    public bool RunRaylib { get; set; } = false;
    [ArgFlag('e', "editor")]    public bool RunEditor { get; set; } = false;
    [ArgFlag('c', "config")]    public bool RunConfig { get; set; } = false;
}


// ReSharper disable once UnusedType.Global
static class Program {
    public static void Main(string[] args) {
        var propertyParser = new PropertyParser<ArgOptions>();
        ArgOptions argOptions = propertyParser.Parse(args);
        
        if (argOptions.ShowHelp) {
            foreach (var helpDescription in propertyParser.GetDescriptionsReadable()) {
                Console.WriteLine(helpDescription);
            }
        } 
        
        var engineLoader = new EngineLoader<Engine2D>();
        var engine = engineLoader.CreateEngine();
        engine.Run();
    }
}
