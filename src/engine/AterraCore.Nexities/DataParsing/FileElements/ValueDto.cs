﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Nexities.DataParsing.FileElements;

using System.Xml.Serialization;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class ValueDto {
    [XmlAttribute("name")]
    public string Name { get; set; }
    
    [XmlAttribute("value")]
    public string Value { get; set; }
}
