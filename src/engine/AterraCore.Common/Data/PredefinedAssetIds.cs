// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;

namespace AterraCore.Common.Data;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class PredefinedAssetIds {
    #region Configuration Warnings
    public static class NewConfigurationWarnings {
        public static readonly AssetId UnstableFlexiPlugLoadOrder = "AterraEngineBoot:Warnings/UnstableFlexiPlugLoadOrder";
        public static readonly AssetId Undefined = "AterraEngineBoot:Warnings/Undefined";
        public static readonly AssetId UnstableBootOperationOrder = "AterraEngineBoot:Warnings/UnstableBootOperationOrder";
        public static readonly AssetId UnableToLoadEngineConfigFile = "AterraEngineBoot:Warnings/UnableToLoadEngineConfigFile";
    }
    #endregion
    
    #region Boot Operations
    public static class NewBootOperationNames {
        public static readonly AssetId EngineConfigLoaderOperation = "AterraEngineBoot:Operations/EngineConfigLoader";
        public static readonly AssetId RegisterWarningsOperation = "AterraEngineBoot:Operations/RegisterWarnings";
    }
    #endregion
}
