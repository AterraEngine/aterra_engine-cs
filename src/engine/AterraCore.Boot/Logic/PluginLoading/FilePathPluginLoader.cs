// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.PluginConfig;
using AterraCore.Common.Types.FlexiPlug;
using AterraCore.Contracts.Boot.FlexiPlug;
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
            
            // ReSharper disable once SuggestVarOrType_Elsewhere
            foreach (var action in actions) {
                if (plugin.Validity is not PluginValidity.Valid) continue;
                action(this, plugin, zipImporter);
            }
        }
        
        return this;
    }
    public IEnumerable<IFilePathLoadedPluginDto> GetValidPlugins() => Plugins
        .Where(plugin => plugin.Validity is PluginValidity.Valid);
}
