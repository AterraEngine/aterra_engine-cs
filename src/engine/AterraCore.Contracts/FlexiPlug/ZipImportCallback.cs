// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.PluginConfig;
using AterraCore.Contracts.Boot.FlexiPlug;

namespace AterraCore.Contracts.FlexiPlug;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public delegate IPreLoadedPluginDto ZipImportCallback(IPreLoadedPluginDto pluginData, IPluginZipImporter<PluginConfigXml> zipImporter);