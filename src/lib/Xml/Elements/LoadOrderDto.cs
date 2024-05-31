﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace Xml.Elements;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class LoadOrderDto {
    [XmlAttribute("breakOnUnstable")]
    public bool BreakOnUnstable { get; set; } = true;
    
    [XmlElement("file")]
    public FileDto[] Plugins { get; set; } = [];
}
