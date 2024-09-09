// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Data;
using System.Xml.Serialization;

namespace AterraCore.Common.ConfigFiles;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[XmlRoot("engineConfig", Namespace = XmlNameSpaces.ConfigEngine)]
public class EngineConfigXml {
    [XmlElement("logging")] public LoggingConfigDto LoggingConfig { get; set; } = new();
    [XmlElement("paths")] public PathsDto? Paths { get; set; }
    [XmlElement("window")] public WindowDto Window { get; set; } = new();

    [XmlElement("loadOrder")] public LoadOrderDto LoadOrder { get; set; } = new();

    // -----------------------------------------------------------------------------------------------------------------
    // Nested classes
    // -----------------------------------------------------------------------------------------------------------------
    // For reasons, I shall use nested classes
    // Yes these reasons are undefined
    public class LoadOrderDto {
        [XmlAttribute("relativeRootPath")] public string RootFolderRelative { get; set; } = Data.Paths.Plugins.Folder;
        [XmlAttribute("enforcePluginIdUniqueness")] public bool EnforcePluginIdUniqueness { get; set; }

        [XmlElement("plugin")] public PluginDto[] Plugins { get; set; } = [];

        public class PluginDto {
            [XmlAttribute("file")] public required string FileName { get; set; }
        }
    }

    public class LoggingConfigDto {
        [XmlAttribute("asyncConsole")] public bool UseAsyncConsole { get; set; } = true;
        [XmlAttribute("outputFile")] public string? OutputFilePath { get; set; } = string.Empty;
    }

    public class PathsDto {
        [XmlElement("gameConfig")] public string? GameConfigPath { get; set; }
    }

    public class WindowDto {
        [XmlAttribute("monitorId")] public int MonitorId { get; set; }
        [XmlAttribute("width")] public int Width { get; set; } = 1000;
        [XmlAttribute("height")] public int Height { get; set; } = 1000;
        [XmlAttribute("name")] public string Name { get; set; } = "AterraEngine";
    }
}
