// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Xml.Serialization;

namespace AterraCore.Common.ConfigFiles.EngineConfig.Elements;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class BootConfigDto {
    [XmlElement("logging")] public BootLoggingDto Logging { get; set; } = new();
    [XmlElement("exceptions")] public BootConfigWarningsDto Exceptions { get; set; } = new();
}
