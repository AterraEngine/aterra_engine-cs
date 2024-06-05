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
    [XmlElement("expections")] public BootExceptionsDto Exceptions { get; set; } = new();
}
