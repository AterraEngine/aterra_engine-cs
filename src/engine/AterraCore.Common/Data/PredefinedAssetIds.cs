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
        public static readonly AssetId DuplicateInPluginLoadOrder = "AterraEngineBoot:Warnings/DuplicateInPluginLoadOrder";
        public static readonly AssetId NoPluginConfigXmlFound = "AterraEngineBoot:Warnings/NoPluginConfigXmlFound";
        public static readonly AssetId AssemblyCouldNotBeLoaded = "AterraEngineBoot:Warnings/AssemblyCouldNotBeLoaded";
        public static readonly AssetId Undefined = "AterraEngineBoot:Warnings/Undefined";
        public static readonly AssetId UnstableBootOperationOrder = "AterraEngineBoot:Warnings/UnstableBootOperationOrder";
        public static readonly AssetId UnableToLoadEngineConfigFile = "AterraEngineBoot:Warnings/UnableToLoadEngineConfigFile";
        public static readonly AssetId EngineOverwritten = "AterraEngineBoot:Warnings/EngineOverwritten";
    }
    #endregion
}
