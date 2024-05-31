﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Xml.Serialization;

namespace AterraCore.Nexities.Parsers.FileElements;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class RequiredPluginDto {
    [XmlAttribute("readableName")]
    public string? ReadableName { get; set; }

    [XmlAttribute("refId")]
    public string? InternalRefId { get; set; }
}
