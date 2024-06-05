// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Xml.Serialization;

namespace AterraCore.Common.ConfigFiles.EngineConfig.Elements;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class BootExceptionsDto {
    [XmlAttribute("breakOnFlowException")] public bool BreakOnFlowException { get; set; } = true;
    [XmlAttribute("breakOnUnstableLoadOrder")] public bool BreakOnUnstableLoadOrder { get; set; } = true;
}
