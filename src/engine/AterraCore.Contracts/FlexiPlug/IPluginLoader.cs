// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.PluginConfig;
using AterraCore.Contracts.Boot.Logic.PluginLoading;

namespace AterraCore.Contracts.FlexiPlug;
// ---------------------------------------------------------------------------------------------------------------------
// Support Code
// ---------------------------------------------------------------------------------------------------------------------
using ZipImporterAction = Action<IFilePathPluginLoader, IFilePathLoadedPluginDto, IPluginZipImporter<PluginConfigXml>>;
using PluginAction = Action<IFilePathPluginLoader, IFilePathLoadedPluginDto>;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IFilePathPluginLoader {
    public LinkedList<IFilePathLoadedPluginDto> Plugins { get; }
    public HashSet<string> Checksums { get; }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IFilePathPluginLoader IterateOverValid(PluginAction action);
    public IFilePathPluginLoader IterateOverValidWithZipImporter(params ZipImporterAction[] actions);
    public IEnumerable<IFilePathLoadedPluginDto> GetValidPlugins();
}
