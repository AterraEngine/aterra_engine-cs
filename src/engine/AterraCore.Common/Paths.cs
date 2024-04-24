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
    public const string Logs = @"Logs\";
    
    private const string _startupLog = "log-startup.db";
    public static readonly string StartupLog = Path.Combine(Logs, _startupLog);
    private const string _engineLog = "log-engine.db";
    public static readonly string EngineLog = Path.Combine(Logs, _engineLog);

    public const string StartupConfig = @"engine-config.xml";

    public const string XsdEngineConfigDto = "xsd/EngineConfigDto.xsd";
    public const string XsdPluginConfigDto = "xsd/PluginConfigDto.xsd";
    public const string PluginsFolder = "plugins";
    public const string PluginConfigPath = "plugin-config.xml";

    public const string PluginDlls = "bin";
}