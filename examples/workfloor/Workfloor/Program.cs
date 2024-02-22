// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine_Workfloor.CliCommands;

namespace AterraEngine_Workfloor;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
static class Program {
    public static void Main(string[] args) {
        IEnumerable<string> enumerable = args.Append("run");
        const  string cliPluginsFolder = @"resources/cli-plugins";
        
        new CliArgsParser.CliArgsParser()
            .RegisterFromCliAtlas(new AterraEngineCommands())
            .RegisterFromDlLs(Directory.GetFiles(cliPluginsFolder, "*.dll"))
            .TryParse(enumerable, true);
    }
}