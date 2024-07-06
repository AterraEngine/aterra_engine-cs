// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.EngineConfig;
using AterraCore.Common.Data;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Boot;
using AterraCore.Loggers;
using CodeOfChaos.Extensions;
using Serilog;
using Xml;
using static AterraCore.Common.Data.PredefinedAssetIds.NewBootOperationNames;
using static AterraCore.Common.Data.PredefinedAssetIds.NewConfigurationWarnings;

namespace AterraCore.Boot.Operations;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class EngineConfigLoader(string? configFilePath = null) : IBootOperation {
    public AssetId AssetId => EngineConfigLoaderOperation;
    private ILogger Logger { get; } = StartupLogger.CreateLogger(false).ForContext("Section", "BO : EngineConfigLoader"); 

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Run(BootOperationComponents components) {
        Logger.Debug("Entered Engine Config Loader");
        
        XmlParser<EngineConfigXml> configXmlParser = new(
            Logger,
            XmlNameSpaces.ConfigEngine, 
            Paths.Xsd.XsdEngineConfigDto
        );

        string filepath = configFilePath.IsNullOrEmpty()
            ? Paths.ConfigEngine
            : configFilePath!;
        
        if (!configXmlParser.TryDeserializeFromFile(filepath, out EngineConfigXml? configDto)) {
            components.WarningAtlas.RaiseWarningEvent(UnableToLoadEngineConfigFile);
            return;
        }
        components.EngineConfigXml = configDto; 
        
    }
}
