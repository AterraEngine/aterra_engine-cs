// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Xml.Serialization;

namespace AterraCore.Common.ConfigFiles.EngineConfig.Elements;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class BootConfigWarningsDto {
    [XmlElement("warning")] public BootWarningDto[]? OriginalWarnings { get; set; } = [];
    [XmlElement("error")] public BootWarningDto[]? OriginalErrors { get; set; } = [];

    [XmlIgnore] public IEnumerable<BootWarningDto> Warnings => OriginalWarnings?.ToHashSet() ?? [];
    [XmlIgnore] public IEnumerable<BootWarningDto> Errors => OriginalErrors?.ToHashSet() ?? [];
}
