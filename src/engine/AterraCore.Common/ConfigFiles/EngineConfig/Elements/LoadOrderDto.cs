// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Xml.Serialization;
using Xml.Elements;

namespace AterraCore.Common.ConfigFiles.EngineConfig.Elements;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class LoadOrderDto {
    [XmlAttribute("breakOnUnstable")]
    public bool BreakOnUnstable { get; set; } = true;

    [XmlAttribute("includeRootAssembly")]
    public bool IncludeRootAssembly { get; set; } = true;

    [XmlElement("rootAssembly", IsNullable = true)]
    public RootAssembly? RootAssembly { get; set; }
    
    [XmlElement("file")]
    public FileDto[] Plugins { get; set; } = [];
}
