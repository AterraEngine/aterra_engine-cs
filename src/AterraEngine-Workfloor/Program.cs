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
        else if (argOptions.RunRaylib) {
            var raylibTest = new SpriteExplosion();
            raylibTest.Main();
        }
        
        // EngineConfig Test
        EngineConfigParser<EngineConfig> engineConfigParser = new EngineConfigParser<EngineConfig>();
        const string engineConfigXml = "resources/engine_config-example.xml";
        const string engineConfigXmlOutput = "resources/engine_config-output.xml";
        
        using StreamReader reader1 = new StreamReader(engineConfigXml);
        Console.WriteLine(reader1.ReadToEnd());
        
        if (
            !engineConfigParser.TryDeserializeFromFile(engineConfigXml, out EngineConfig? engineConfig)
            || engineConfig is null) {
            throw new Exception("File coule not be parsed");
        }
        
        // Console.WriteLine(engineConfig.Version);
        
        Console.WriteLine(engineConfig.Plugins.Count);
        
        // engineConfig.Version = new SemanticVersion("0.0.1");
        engineConfig.Plugins.ForEach(config => Console.WriteLine(config));
        Console.WriteLine(engineConfig.Plugins.Count);
        engineConfigParser.TrySerializeToFile(engineConfig, engineConfigXmlOutput);
        
        // sshh this is not a good idea in the long run 
        using StreamReader reader2 = new StreamReader(engineConfigXmlOutput);
        Console.WriteLine(reader2.ReadToEnd());

    }
}
