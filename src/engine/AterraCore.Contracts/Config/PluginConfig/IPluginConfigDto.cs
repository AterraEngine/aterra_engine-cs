// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common;
using AterraCore.Contracts.Config.Xml;

namespace AterraCore.Contracts.Config.PluginConfig;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IPluginConfigDto{
    string ReadableName { get; set; }
    string Author { get; set; }
    SemanticVersion PluginVersion { get; }
    SemanticVersion GameVersion { get; }
    IEnumerable<IFileDto> Dlls { get; }
}