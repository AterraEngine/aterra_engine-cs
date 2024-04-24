﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Xml.Serialization;

namespace AterraCore.Config.Xml;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class RaylibWindowElementDto {
    [XmlElement("screen")] public required DimensionElementDto Screen { get; set; }
    [XmlElement("icon-path")]   public required string IconPath { get; set; }
    [XmlElement("title")]  public required string Title { get; set; }
}