﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Xml.Serialization;

namespace AterraEngine.Config.Elements;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class LoggingDto {
    [XmlAttribute("async-console")] public bool UseAsyncConsole { get; set; }
}