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

    [XmlElement("loadOrder")] 
    public required LoadOrderDto LoadOrder { get; set; }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override string ToString() {
        var txt = new StringBuilder();
        txt.Append($"RootFolder : {RootFolder} ");
        txt.Append("Plugins : ");
        LoadOrder.Plugins.IterateOver(p => txt.Append($"{p} ,"));
        return txt.ToString();
    }
}