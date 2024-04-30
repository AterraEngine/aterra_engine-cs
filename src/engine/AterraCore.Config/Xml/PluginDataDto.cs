// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Text;
using System.Xml.Serialization;
using AterraCore.Extensions;

namespace AterraCore.Config.Xml;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class PluginDataDto {
    [XmlAttribute("root")]
    public required string RootFolder { get; set; }
    
    [XmlArray("LoadOrder")]
    [XmlArrayItem("Plugin", typeof(FileDto))]
    public required FileDto[] Plugins { get; set; } 

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override string ToString() {
        var txt = new StringBuilder();
        txt.Append($"RootFolder : {RootFolder} ");
        txt.Append("Plugins : ");
        Plugins.IterateOver(p => txt.Append($"{p} ,"));
        return txt.ToString();
    }
}