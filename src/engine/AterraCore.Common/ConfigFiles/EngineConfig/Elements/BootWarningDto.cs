// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using System.Xml.Serialization;

namespace AterraCore.Common.ConfigFiles.EngineConfig.Elements;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class BootWarningDto {
    [XmlAttribute("id")] public string? StringAssetId { get; set; }
    [XmlIgnore] public AssetId AssetId => StringAssetId!;

    public override int GetHashCode() => AssetId.GetHashCode();
    
}
