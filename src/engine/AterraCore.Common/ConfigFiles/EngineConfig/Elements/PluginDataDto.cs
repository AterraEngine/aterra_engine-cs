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

public class PluginDataDto {
    [XmlAttribute("root")]
    public string RootFolder { get; set; } = Paths.Plugins.Folder;

    [XmlElement("loadOrder")]
    public LoadOrderDto LoadOrder { get; set; } = new();

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
