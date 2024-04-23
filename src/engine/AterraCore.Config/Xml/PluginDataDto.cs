// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Text;
using System.Xml.Serialization;
using AterraCore.Common;

namespace AterraCore.Config.Xml;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class PluginDataDto {
    [XmlElement("RootFolder")]
    public required string RootFolder { get; set; }
    
    [XmlArray("Plugins")]
    [XmlArrayItem("Plugin", typeof(PluginRecordDto))]
    public required List<PluginRecordDto> Plugins { get; set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override string ToString() {
        var txt = new StringBuilder();
        txt.Append($"RootFolder : {RootFolder} ");
        txt.Append($"Plugins : ");
        Plugins.ForEach(p => txt.Append($"{p} ,"));
        return txt.ToString();
    }
}