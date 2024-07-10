// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.PluginConfig;
using AterraCore.Contracts.Boot.FlexiPlug;

namespace AterraCore.Contracts.FlexiPlug;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public delegate IFilePathLoadedPluginDto ZipImportCallback(IFilePathLoadedPluginDto pluginData, IPluginZipImporter<PluginConfigXml> zipImporter);