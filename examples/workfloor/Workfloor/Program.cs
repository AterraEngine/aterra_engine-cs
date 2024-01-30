// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine_Workfloor.CliCommands;
using CliCommandParser;

namespace AterraEngine_Workfloor;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
static class Program {
    public static void Main(string[] args) {
        const  string cliPluginsFolder = @"resources/cli-plugins";
        
        new CliParser()
            .RegisterFromCliAtlas(new AterraEngineCommands())
            .RegisterFromDlLs(Directory.GetFiles(cliPluginsFolder, "*.dll"))
            .TryParse(args, true);
    }
}