// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Data;
using System.Xml.Serialization;
using Xml.Elements;

namespace AterraCore.Common.ConfigFiles.EngineConfig.Elements;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LoadOrderDto {
    [XmlAttribute("relativeRootPath")] public string RootFolderRelative { get; set; } = Paths.Plugins.Folder;
    [XmlElement("plugin")] public FileDto[] Plugins { get; set; } = [];
}
