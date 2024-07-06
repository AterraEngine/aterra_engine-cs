// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Common.Data;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class Paths {
    public const string ConfigEngine = "engine-config.xml";

    public static class Logs {
        public const string Folder = "logs";
        private const string _startupLog = "log_startup-.log";
        private const string _engineLog = "log_engine-.log";
        private const string _rendererLog = "log_renderer-.log";
        public static readonly string StartupLog = Path.Combine(Folder, _startupLog);
        public static readonly string EngineLog = Path.Combine(Folder, _engineLog);
        public static readonly string RendererLog = Path.Combine(Folder, _rendererLog);
    }

    public static class Plugins {
        public const string Folder = "plugins";
        public const string PluginConfig = "plugin-config.xml";
        public const string PluginBinFolder = "bin";
        public const string PluginAssetsFolder = "assets";
        public const string PluginResourcesFolder = "res";
    }

    public static class Xsd {
        public const string Folder = "xsd";
        private const string _xsdEngineConfigDto = "engine-config.xsd";
        private const string _xsdPluginConfigDto = "plugin-config.xsd";
        private const string _xsdAssetDataDto = "asset-data.xsd";
        public static readonly string XsdEngineConfigDto = Path.Combine(Folder, _xsdEngineConfigDto);
        public static readonly string XsdPluginConfigDto = Path.Combine(Folder, _xsdPluginConfigDto);
        public static readonly string XsdAssetDataDto = Path.Combine(Folder, _xsdAssetDataDto);
    }
}
