// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Xml.Serialization;

namespace AterraCore.Common.ConfigFiles.EngineConfig.Elements;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class BootExceptionsDto {
    [XmlAttribute("breakOnUnstableLoadOrder")] public bool BreakOnUnstableLoadOrder { get; set; } = true;
    [XmlAttribute("breakOnOverwriteOfEngine")] public bool BreakOnOverwriteOfEngine { get; set; } = true;
}
