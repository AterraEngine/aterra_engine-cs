// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.EngineConfig;
using AterraCore.Common.Data;
using AterraCore.Common.Types.Nexities;
using AterraCore.Loggers;
using CodeOfChaos.Extensions;
using JetBrains.Annotations;
using Xml;
using static AterraCore.Common.Data.PredefinedAssetIds.NewBootOperationNames;
using static AterraCore.Common.Data.PredefinedAssetIds.NewConfigurationWarnings;

namespace AterraCore.Boot.Operations;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class EngineConfigLoader : IBootOperation {
    public AssetId AssetId => EngineConfigLoaderOperation;
    public AssetId? RanAfter => null;
    private ILogger Logger { get; } = StartupLogger.CreateLogger(false).ForBootOperationContext<EngineConfigLoader>();
    private readonly string? _configFilePath;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    [UsedImplicitly] public EngineConfigLoader(){}
    [UsedImplicitly] public EngineConfigLoader(string? configFilePath = null) {
        _configFilePath = configFilePath;
    }
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Run(IBootOperationComponents components) {
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
            components.WarningAtlas.RaiseEvent(UnableToLoadEngineConfigFile);
            return;
        }
        components.EngineConfigXml = configDto; 
        
    }
}
