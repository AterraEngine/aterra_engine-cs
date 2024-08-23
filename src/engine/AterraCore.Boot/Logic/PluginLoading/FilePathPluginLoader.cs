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
using ZipImporterAction = Action<IFilePathPluginLoader, IFilePathLoadedPluginDto, IPluginZipImporter<PluginConfigXml>>;
using PluginAction = Action<IFilePathPluginLoader, IFilePathLoadedPluginDto>;
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
    public IFilePathPluginLoader IterateOverValid(PluginAction action) {
        foreach (IFilePathLoadedPluginDto plugin in GetValidPlugins()) action(this, plugin);
        return this;
    }
    
    
    public IFilePathPluginLoader IterateOverValidWithZipImporter(params ZipImporterAction[] actions) {
        foreach (IFilePathLoadedPluginDto plugin in GetValidPlugins()) {
            using var zipImporter = new PluginZipImporter(plugin.FilePath, logger);
            foreach (ZipImporterAction action in actions) {
                action(this, plugin, zipImporter);
                // Break away from the zip importer if plugin became invalid
                if (!plugin.IsValid) break; 
            }
        }
        
        return this;
    }
    public IEnumerable<IFilePathLoadedPluginDto> GetValidPlugins() => 
        Plugins.Where(plugin => plugin.IsValid);
}
