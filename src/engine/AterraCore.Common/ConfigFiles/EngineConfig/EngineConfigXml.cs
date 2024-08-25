// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.EngineConfig.Elements;
using System.Xml.Serialization;

namespace AterraCore.Common.ConfigFiles.EngineConfig;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[XmlRoot("engineConfig")]
public class EngineConfigXml {
    [XmlElement("engine")] public EngineDto Engine { get; set; } = new();
    [XmlElement("game")] public GameDto Game { get; set; } = new();
    [XmlElement("boot")] public BootConfigDto BootConfig { get; set; } = new();
    [XmlElement("loadOrder")] public LoadOrderDto LoadOrder { get; set; } = new();
    [XmlElement("raylib")] public RaylibConfigDto RaylibConfig { get; set; } = new();
}
