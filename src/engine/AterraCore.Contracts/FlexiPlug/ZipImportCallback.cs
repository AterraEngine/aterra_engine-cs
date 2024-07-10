// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.PluginConfig;
using AterraCore.Contracts.Boot.Logic.PluginLoading;

namespace AterraCore.Contracts.FlexiPlug;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public delegate IFilePathLoadedPluginDto ZipImportCallback(IFilePathLoadedPluginDto pluginData, IPluginZipImporter<PluginConfigXml> zipImporter);