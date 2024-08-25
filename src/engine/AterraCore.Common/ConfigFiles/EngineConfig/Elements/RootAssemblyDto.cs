// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Xml.Serialization;

namespace AterraCore.Common.ConfigFiles.EngineConfig.Elements;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class RootAssemblyDto {
    [XmlAttribute("author")] public string Author { get; set; } = string.Empty;
    [XmlAttribute("nameSpace")] public string NameSpace { get; set; } = string.Empty;
    [XmlAttribute("nameReadable")] public string NameReadable { get; set; } = string.Empty;
}
