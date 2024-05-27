﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Nexities.Parsers.FileElements;

using Common.Types.Nexities;
using System.Xml.Serialization;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class ComponentDto {
    
    [XmlAttribute("assetId")]
    public string? RawAssetId { get; set; }

    [XmlElement("value", typeof(ValueDto))]
    public ValueDto[] Values { get; set; } = [];
}
