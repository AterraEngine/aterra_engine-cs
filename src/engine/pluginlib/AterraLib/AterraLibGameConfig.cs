// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Attributes.ConfigMancer;
using AterraLib.Contracts;
using System.Xml.Serialization;

namespace AterraLib;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[ConfigMancerElement(AssetIdLib.AterraLib.ConfigMancer)]
[XmlRoot("aterraLib", Namespace = XmlNameSpaces.ConfigGameAterraLib)]
public class AterraLibGameConfig : IAterraLibGameConfig {
    [XmlElement("somePupperty")] public string SomePupperty { get; set; } = string.Empty;
}