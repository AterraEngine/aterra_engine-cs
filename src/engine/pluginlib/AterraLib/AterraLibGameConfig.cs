// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraLib.Contracts;
using System.Xml.Serialization;

namespace AterraLib;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[XmlRoot("aterraLib", Namespace = XmlNameSpaces.ConfigGameAterraLib)]
public class AterraLibGameConfig : IAterraLibGameConfig {
    [XmlElement("somePupperty")] public string SomePupperty { get; set; } = string.Empty;
}