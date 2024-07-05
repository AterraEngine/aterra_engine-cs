// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Xml.Serialization;
using Xml.Elements;

namespace AterraCore.Common.ConfigFiles.EngineConfig.Elements;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class RaylibWindowElementDto {
    [XmlElement("screen")] public DimensionElementDto Screen { get; set; } = new();
    [XmlElement("icon-path")] public string? IconPath { get; set; }
    [XmlElement("title")] public string Title { get; set; } = "Aterra Engine";
}
