// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles;
using AterraCore.Common.Data;
using AterraCore.Contracts.Boot.Operations;
using AterraCore.Loggers;
using CodeOfChaos.Extensions;
using CodeOfChaos.Extensions.Serilog;
using JetBrains.Annotations;
using System.Configuration;
using Xml;

namespace AterraCore.Boot.Operations;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class EngineConfigLoader : IBootOperation {
    private ILogger Logger { get; } = StartupLogger.CreateLogger(false).ForBootOperationContext<EngineConfigLoader>();
    private readonly string? _configFilePath;

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    [UsedImplicitly] public EngineConfigLoader() {}
    [UsedImplicitly] public EngineConfigLoader(string? configFilePath = null) {
        _configFilePath = configFilePath;
    }
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Run(IBootComponents components) {
        Logger.Debug("Entered Engine Config Loader");

        XmlParser<EngineConfigXml> configXmlParser = new(
            Logger,
            XmlNameSpaces.ConfigEngine,
            Paths.Xsd.XsdEngineConfigDto
        );

        string filepath = _configFilePath.IsNullOrEmpty()
            ? Paths.ConfigEngine
            : _configFilePath!;

        if (!configXmlParser.TryDeserializeFromFile(filepath, out EngineConfigXml? configDto)) {
            Logger.ThrowError<ConfigurationException>("Failed to load Engine Config");
            return;
        }
        components.EngineConfigXml = configDto;

    }
}
