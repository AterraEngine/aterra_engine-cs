// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace ArgsParser.Interfaces;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ICliParser {
    ICliParser RegisterFromCliAtlas<T>(T cliCommandAtlas, bool force = false) where T : ICliCommandAtlas;
    ICliParser RegisterFromDlLs(IEnumerable<string> filePaths);

    bool TryParse(string[] args);
    
    void HelpCommand(string[] args);
}