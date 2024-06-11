// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions;
using System.Xml.Serialization;

namespace AterraCore.Common.ConfigFiles.EngineConfig.Elements;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class BootLoggingDto {
    [XmlAttribute("async-console")] public bool UseAsyncConsole { get; set; } = true;
    
    [XmlAttribute("output-file")] public string? OutputFilePath { get; set; } = string.Empty;
    public bool OutputToFile => OutputFilePath.IsNotNullOrEmpty();
}
