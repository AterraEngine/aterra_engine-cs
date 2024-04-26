// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Loggers;
using CliArgsParser;
using ProductionTools.Commands;
using Serilog;
using Serilog.Core;

namespace ProductionTools;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public static class Program {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static void Main(string[] args) {
        using Logger logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .DefaultSinkConsole() // Using the normal version of the Sink Console, else the empty lines get processed earlier.
            .CreateLogger();
        
        List<CliCommandAtlas> commandAtlasArray = [
            new XmlSchemaGenerator(logger),
            new TestConsoleTheme(logger)
        ];
        
        var argsParser = new CliArgsParser.CliArgsParser();
        
        commandAtlasArray.ForEach(a => argsParser.RegisterFromCliAtlas(a));
            
        argsParser.TryParseMultiple(args);
    }
}