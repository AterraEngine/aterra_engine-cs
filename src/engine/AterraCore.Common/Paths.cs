﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Common;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class Paths {
    public static class Logs {
        public const string Folder = "logs";
        private const string _startupLog = "log-startup.db";
        public static readonly string StartupLog = Path.Combine(Folder, _startupLog);
        private const string _engineLog = "log-engine.db";
        public static readonly string EngineLog = Path.Combine(Folder, _engineLog);
    }
    
    public static class Plugins {
        public const string Folder = "plugins";
        public const string PluginConfig = "plugin-config.xml";
        public const string PluginBinFolder = "bin";
    }

    public static class Xsd {
        public const string Folder = "xsd";
        private const string _xsdEngineConfigDto = "EngineConfigDto.xsd";
        public static readonly string XsdEngineConfigDto = Path.Combine(Folder, _xsdEngineConfigDto);
        private const string _xsdPluginConfigDto = "PluginConfigDto.xsd";
        public static readonly string XsdPluginConfigDto = Path.Combine(Folder, _xsdPluginConfigDto);
        private const string _xsdGameConfigDto = "GameConfigDto.xsd";
        public static readonly string XsdGameConfigDto = Path.Combine(Folder, _xsdGameConfigDto);
    }
    
    public const string StartupConfig = "engine-config.xml";
}