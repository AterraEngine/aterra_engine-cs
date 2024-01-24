// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace ArgsParser.Interfaces;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ICliParser {
    ICliParser RegisterCli<T>(T cliCommandAtlas, bool force = false) where T : ICliCommandAtlas;
    ICliParser ImportFromDlLs(IEnumerable<string> filePaths);

    bool TryParse(string[] args);
    
    void HelpCommand(string[] args);
}