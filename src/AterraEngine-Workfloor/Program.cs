// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using ArgsParser;
namespace AterraEngine_Workfloor;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ArgOptions {
    [ArgFlag('h', "help")]      public bool ShowHelp { get; set; } = false;
    [ArgFlag('v', "version")]   public bool ShowVersion { get; set; } = false;
    [ArgFlag('d', "debug")]     public bool EnableDebug { get; set; } = false;
    
    [ArgFlag('e', "editor")]    public bool RunEditor { get; set; } = false;
}

// ReSharper disable once UnusedType.Global
static class Program {
    public static void Main(string[] args) {
        var propertyParser = new PropertyParser<ArgOptions>();
        ArgOptions argOptions = propertyParser.Parse(args);

        if (argOptions.ShowHelp) {
            foreach (var helpDescription in propertyParser.GetReadableDescriptions()) {
                Console.WriteLine(helpDescription);
            }
        }
    }
}
