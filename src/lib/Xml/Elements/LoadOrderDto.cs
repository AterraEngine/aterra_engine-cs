﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace Xml.Elements;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class LoadOrderDto {
    [XmlAttribute("breakOnUnstable")]
    public bool BreakOnUnstable { get; set; }

    [XmlElement("file")]
    public required FileDto[] Plugins { get; set; }
}