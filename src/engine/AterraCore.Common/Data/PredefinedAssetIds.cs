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
    
    #region Boot Operations
    public static class NewBootOperationNames {
        public static readonly AssetId EngineConfigLoaderOperation = "AterraEngineBoot:Operations/EngineConfigLoader";
        public static readonly AssetId RegisterWarningsOperation = "AterraEngineBoot:Operations/RegisterWarnings";
        public static readonly AssetId UseEngineOperation = "AterraEngineBoot:Operations/UseEngine";
        public static readonly AssetId CollectDependenciesOperation = "AterraEngineBoot:Operations/CollectDependencies";
        public static readonly AssetId BuildDependenciesOperation = "AterraEngineBoot:Operations/BuildDependencies";
        public static readonly AssetId PluginLoaderDefineOperation = "AterraEngineBoot:Operations/PluginLoader/Define";
        public static readonly AssetId PluginLoaderPreChecksOperation = "AterraEngineBoot:Operations/PluginLoader/PreChecks";
        public static readonly AssetId PluginLoaderImporterOperation = "AterraEngineBoot:Operations/PluginLoader/Importer";
    }
    #endregion
}
