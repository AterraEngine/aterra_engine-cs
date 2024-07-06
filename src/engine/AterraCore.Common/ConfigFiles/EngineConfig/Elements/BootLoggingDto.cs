// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Xml.Serialization;

namespace AterraCore.Common.ConfigFiles.EngineConfig.Elements;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class BootLoggingDto {
    [XmlAttribute("asyncConsole")] public bool UseAsyncConsole { get; set; } = true;
    [XmlAttribute("outputFile")] public string? OutputFilePath { get; set; } = string.Empty;
}
