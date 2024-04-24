// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common;

namespace AterraCore.Contracts.Config.PluginConfig;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IPluginConfigDto<out T> where T : ISemanticVersion{
    string ReadableName { get; set; }
    string? Author { get; set; }
    T PluginVersion { get; }
    T GameVersion { get; }
    List<string> Dlls { get; set; }
}