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
    private ILogger Logger { get; } = StartupLogger.CreateLogger(false).ForContext("Context", "BO : RegisterWarnings"); 

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Run(BootOperationComponents components) {
        Logger.Debug("Entered Register Warnings");

        if (components.EngineConfigXml.BootConfig.Exceptions.BreakOnUnstableLoadOrder) {
            components.WarningAtlas.AddWarningEvent(UnstableFlexiPlugLoadOrder, (sender, args) => {
                Logger.ExitFatal(
                    (int)ExitCodes.UnstableFlexiPlugLoadOrder,
                    args.Warning.MessageTemplate ?? "FlexiPlug's Load order was unstable. Triggered by {sender}",
                    sender
                );  
            });
        }
    }
}
