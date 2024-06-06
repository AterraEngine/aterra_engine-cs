﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Xml.Serialization;

namespace AterraCore.Common.ConfigFiles.EngineConfig.Elements;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class RootAssemblyDto {
    [XmlAttribute("author")]
    public string Author { get; set; } = string.Empty;
    
    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;
}