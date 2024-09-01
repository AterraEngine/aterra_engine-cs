// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles;
using AterraCore.Contracts.Boot.Logic.PluginLoading.Dto;

namespace AterraCore.Contracts.FlexiPlug;

// ---------------------------------------------------------------------------------------------------------------------
// Support Code
// ---------------------------------------------------------------------------------------------------------------------
using ZipImporterAction=Action<IFilePathPluginLoader, IPluginBootDto, IPluginZipImporter<PluginConfigXml>>;
using PluginAction=Action<IFilePathPluginLoader, IRawPluginBootDto, IPluginBootDto>;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IFilePathPluginLoader {
    public LinkedList<(IRawPluginBootDto rawPluginBootDto, IPluginBootDto pluginBootDto)> Plugins { get; }
    public HashSet<string> Checksums { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IFilePathPluginLoader IterateOverValid(PluginAction action);
    public IFilePathPluginLoader IterateOverValidWithZipImporter(params ZipImporterAction[] actions);
    public IEnumerable<(IRawPluginBootDto rawPluginBootDto, IPluginBootDto pluginBootDto)> GetValidPlugins();
}
