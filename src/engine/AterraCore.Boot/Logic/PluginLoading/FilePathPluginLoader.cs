// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.PluginConfig;
using AterraCore.Contracts.Boot.Logic.PluginLoading;
using AterraCore.Contracts.FlexiPlug;

namespace AterraCore.Boot.Logic.PluginLoading;
// ---------------------------------------------------------------------------------------------------------------------
// Support Code
// ---------------------------------------------------------------------------------------------------------------------
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class FilePathPluginLoader(ILogger logger) : IFilePathPluginLoader {
    private ILogger Logger { get; } = logger.ForContext<FilePathPluginLoader>();
    public LinkedList<IFilePathLoadedPluginDto> Plugins { get; } = [];
    public HashSet<string> Checksums { get; } = [];

    // -----------------------------------------------------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IFilePathPluginLoader IterateOverValid(Action<IFilePathPluginLoader, IFilePathLoadedPluginDto> action) {
        foreach (IFilePathLoadedPluginDto plugin in GetValidPlugins()) {
            action(this, plugin);
        }
        return this;
    }
    
    
    public IFilePathPluginLoader IterateOverValidWithZipImporter(params Action<IFilePathPluginLoader, IFilePathLoadedPluginDto, IPluginZipImporter<PluginConfigXml>>[] actions) {
        foreach (IFilePathLoadedPluginDto plugin in GetValidPlugins()) {
            using var zipImporter = new PluginZipImporter(plugin.FilePath, logger);
            
            // Just, I know this isn't "standard", but save yourself a "too long line headache"
            //      Look at the params type of this method if you really want to know the type.
            // ReSharper disable once SuggestVarOrType_Elsewhere
            foreach (var action in actions) {
                action(this, plugin, zipImporter);
            }
        }
        
        return this;
    }
    public IEnumerable<IFilePathLoadedPluginDto> GetValidPlugins() => Plugins;
}
