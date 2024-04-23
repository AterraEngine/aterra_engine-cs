// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using CliArgsParser.Contracts;
using ProductionTools.Commands;

namespace ProductionTools;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public static class Program {
    private static readonly List<ICliCommandAtlas> CommandAtlasArray = [
        new XmlSchemaGenerator()
    ];
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static void Main(string[] args) {
        var argsParser = new CliArgsParser.CliArgsParser();
        
        CommandAtlasArray.ForEach(a => argsParser.RegisterFromCliAtlas(a));
            
        argsParser.TryParseMultiple(args);
    }
}