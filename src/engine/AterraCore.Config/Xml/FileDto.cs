// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Xml.Serialization;
using AterraCore.Contracts.Config.Xml;

namespace AterraCore.Config.Xml;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class FileDto : IFileDto {
    [XmlAttribute("file")] 
    public required string FileNameInternal { get; set; }

    [XmlIgnore]
    public string FilePath { get; set; } = null!;
}