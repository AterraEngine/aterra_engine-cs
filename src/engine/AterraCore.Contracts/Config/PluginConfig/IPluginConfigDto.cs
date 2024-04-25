// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common;

namespace AterraCore.Contracts.Config.PluginConfig;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IPluginConfigDto{
    string ReadableName { get; set; }
    string Author { get; set; }
    SemanticVersion PluginVersion { get; }
    SemanticVersion GameVersion { get; }
    List<string> Dlls { get; set; }
}