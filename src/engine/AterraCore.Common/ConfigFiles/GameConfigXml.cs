// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Data;
using System.Xml;
using System.Xml.Serialization;

namespace AterraCore.Common.ConfigFiles;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[XmlRoot("gameConfig", Namespace = XmlNameSpaces.ConfigGame)]
public class GameConfigXml {
    [XmlAnyElement("configs")]
    public List<XmlNode> Configs { get; set; } = [];
}
