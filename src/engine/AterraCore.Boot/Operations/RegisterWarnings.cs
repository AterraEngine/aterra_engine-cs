// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.EngineConfig.Elements;
using AterraCore.Common.Data;
using AterraCore.Common.Types.Nexities;
using AterraCore.Loggers;
using CodeOfChaos.Extensions.Serilog;
using static AterraCore.Common.Data.PredefinedAssetIds.NewBootOperationNames;
using static AterraCore.Common.Data.PredefinedAssetIds.NewConfigurationWarnings;

namespace AterraCore.Boot.Operations;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class RegisterWarnings : IBootOperation {
    public AssetId AssetId => RegisterWarningsOperation;
    public AssetId? RanAfter => EngineConfigLoaderOperation;
    private ILogger Logger { get; } = StartupLogger.CreateLogger(false).ForBootOperationContext<RegisterWarnings>(); 

    private Dictionary<AssetId, (string warning, string error, ExitCodes exitCodes)> Exceptions { get; } = new() {
        #region UnstableFlexiPlugLoadOrder
        {UnstableFlexiPlugLoadOrder, (
            warning: "FlexiPlug's Load order was unstable",
            error: "FlexiPlug's Load order was unstable",
            exitCodes: ExitCodes.UnstableFlexiPlugLoadOrder
        )},
        #endregion
        #region EngineOverwritten
        {EngineOverwritten, (
            warning: "Engine Type {currentType} overwritten with {newType}",
            error:"Tried to overwrite the Engine Type {currentType} with {newType}",
            exitCodes: ExitCodes.EngineOverwritten
        )},
        #endregion
        #region UnableToLoadEngineConfigFile
        {UnableToLoadEngineConfigFile, (
                warning: "No ConfigFile for the Engine could be found.",
                error:"No ConfigFile for the Engine could be found.",
                exitCodes: ExitCodes.UnableToLoadEngineConfigFile
            )},
        #endregion
        #region NoPluginConfigXmlFound
        {NoPluginConfigXmlFound, (
                warning: "Plugin {filePath} did not have a valid plugin.xml file.",
                error:"Plugin {filePath} did not have a valid plugin.xml file.",
                exitCodes: ExitCodes.UnableToLoadEngineConfigFile
            )},
        #endregion
    };
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Run(IBootOperationComponents components) {
        Logger.Debug("Entered Register Warnings");
        
        #region Non breaking Warnings
        foreach (BootWarningDto dto in components.EngineConfigXml.BootConfig.Exceptions.Warnings) {
            if (!Exceptions.TryGetValue(dto.AssetId, out (string warning, string error, ExitCodes exitCode) exception)) continue;
            components.WarningAtlas.AddEvent(dto.AssetId, (_, args) => {
                Logger.Warning(
                exception.warning,
                args.MessageParams
                );
            });
            Logger.Debug("Registered Warning Event callback for {assetId}", dto.AssetId);
        }
        #endregion
        #region Fatal Errors
        foreach (BootWarningDto dto in components.EngineConfigXml.BootConfig.Exceptions.Errors) {
            if (!Exceptions.TryGetValue(dto.AssetId, out (string warning, string error, ExitCodes exitCode) exception)) continue;
            components.WarningAtlas.AddEvent(dto.AssetId, (_, args) => {
                Logger.ExitFatal(
                (int)exception.exitCode,
                exception.error,
                args.MessageParams
                );
            });
            Logger.Debug("Registered Fatal Event callback for {assetId}", dto.AssetId);
        }
        #endregion
    }
}
