// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Ansi;
using ArgsParser;
namespace AterraEngine_Workfloor;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ArgOptions {
    [ArgFlag('h', "help")] public bool ShowHelp { get; set; } = false;
    [ArgFlag('e', "editor")] public bool RunEditor { get; set; } = false;
    [ArgFlag('v', "version")] public bool ShowVersion { get; set; } = false;
    [ArgFlag('d', "debug")] public bool EnableDebug { get; set; } = false;
    
    
}

// ReSharper disable once UnusedType.Global
static class Program {
    public static void Main(string[] args) {
        ArgOptions argOptions = new PropertyParser<ArgOptions>().Parse(args);

        
    }
}