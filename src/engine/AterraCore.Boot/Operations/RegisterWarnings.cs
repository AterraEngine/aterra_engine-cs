// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.EngineConfig;
using AterraCore.Common.Data;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Boot;
using AterraCore.Loggers;
using CodeOfChaos.Extensions;
using CodeOfChaos.Extensions.Serilog;
using Serilog;
using Xml;
using static AterraCore.Common.Data.PredefinedAssetIds.NewBootOperationNames;
using static AterraCore.Common.Data.PredefinedAssetIds.NewConfigurationWarnings;

namespace AterraCore.Boot.Operations;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class RegisterWarnings : IBootOperation {
    public AssetId AssetId => RegisterWarningsOperation;
    private ILogger Logger { get; } = StartupLogger.CreateLogger(false).ForContext("Section", "BO : RegisterWarnings"); 

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Run(BootOperationComponents components) {
        Logger.Debug("Entered Register Warnings");

        #region UnstableFlexiPlugLoadOrder
        components.WarningAtlas.AddWarningEvent(UnstableFlexiPlugLoadOrder, (sender, args) => {
            if (components.EngineConfigXml.BootConfig.Exceptions.BreakOnUnstableLoadOrder) 
                Logger.ExitFatal(
                    (int)ExitCodes.UnstableFlexiPlugLoadOrder, 
                    args.Warning.MessageTemplate ?? "FlexiPlug's Load order was unstable. Triggered by {sender}",
                    sender
                );
            else 
                Logger.Error(
                    args.Warning.MessageTemplate ?? "FlexiPlug's Load order was unstable. Triggered by {sender}",
                    sender
                );
        });
        Logger.Debug("Registered Warning Event callback for {assetId}", UnstableFlexiPlugLoadOrder);
        #endregion

        #region EngineOverwritten
        components.WarningAtlas.AddWarningEvent(EngineOverwritten, (_, args) => {
            if (components.EngineConfigXml.BootConfig.Exceptions.BreakOnOverwriteOfEngine)
                Logger.ExitFatal(
                    (int)ExitCodes.EngineOverwritten, 
                    args.Warning.MessageTemplate ?? "Tried to overwrite the Engine Type {currentType} with {newType}", 
                    args.MessageParams
                ); 
            else
                Logger.Error(
                    args.Warning.MessageTemplate ?? "Engine Type {currentType} overwritten with {newType}", 
                    args.MessageParams
                );
        });
        Logger.Debug("Registered Warning Event callback for {assetId}", EngineOverwritten);
        #endregion
    }
}
