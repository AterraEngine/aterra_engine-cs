// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Data;
using Extensions;
using System.Text;
using System.Xml.Serialization;
using Xml.Elements;

namespace AterraCore.Common.ConfigFiles.EngineConfig.Elements;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class LoadOrderDto {
    [XmlAttribute("relative-root-path")]
    public string RootFolderRelative { get; set; } = Paths.Plugins.Folder;
    
    [XmlAttribute("includeRootAssembly")]
    public bool IncludeRootAssembly { get; set; } = true;

    [XmlElement("rootAssembly", IsNullable = true)]
    public RootAssemblyDto? RootAssembly { get; set; }
    
    [XmlElement("file")]
    public FileDto[] Plugins { get; set; } = [];
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override string ToString() {
        var txt = new StringBuilder();
        txt.Append($"RootFolder : {RootFolderRelative} ");
        txt.Append("Plugins : ");
        Plugins.IterateOver(p => txt.Append($"{p} ,"));
        return txt.ToString();
    }
}
