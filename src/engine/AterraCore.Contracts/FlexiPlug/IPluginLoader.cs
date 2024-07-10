// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.PluginConfig;
using AterraCore.Contracts.Boot.Logic.PluginLoading;

namespace AterraCore.Contracts.FlexiPlug;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IFilePathPluginLoader {
    public LinkedList<IFilePathLoadedPluginDto> Plugins { get; }
    public HashSet<string> Checksums { get; }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IFilePathPluginLoader IterateOverValid(Action<IFilePathPluginLoader, IFilePathLoadedPluginDto> action);
    public IFilePathPluginLoader IterateOverValidWithZipImporter(params Action<IFilePathPluginLoader, IFilePathLoadedPluginDto, IPluginZipImporter<PluginConfigXml>>[] actions);
    public IEnumerable<IFilePathLoadedPluginDto> GetValidPlugins();
}
