﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace Xml.Elements;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record DimensionElementDto {
    [XmlAttribute("height")] public int Height;
    [XmlAttribute("width")] public int Width;
}
