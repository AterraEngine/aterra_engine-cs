// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.Types;
using Xml.Elements;

namespace AterraCore.Contracts.Boot.FlexiPlug.PluginDtos;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IPluginConfigDto {
    string ReadableName { get; set; }
    string Author { get; set; }
    SemanticVersion PluginVersion { get; }
    SemanticVersion GameVersion { get; }
    IEnumerable<FileDto> Dlls { get; }
}
