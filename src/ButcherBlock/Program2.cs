// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Xml;
using ArgsParser;
using AterraEngine_lib.structs;
using AterraEngine.Config;
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
static class Program2 {
    public static void Maian(string[] args) {
        
        
        
        var propertyParser = new PropertyParser<ArgOptions>();
        ArgOptions argOptions = propertyParser.Parse(args);
        
        if (argOptions.ShowHelp) {
            foreach (var helpDescription in propertyParser.GetDescriptionsReadable()) {
                Console.WriteLine(helpDescription);
            }
        } 
        
        else if (argOptions.RunRaylib) {
            var raylibTest = new SpriteExplosion();
            raylibTest.Main();
        } 
        
        else if (argOptions.RunConfig) {
            new ConfigTest().Main();
        }

    }
}
