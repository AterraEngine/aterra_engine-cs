// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.PluginConfig;
using AterraCore.Common.Types.FlexiPlug;
using AterraCore.Contracts.Boot.FlexiPlug;
using AterraCore.Contracts.FlexiPlug;
using CodeOfChaos.Extensions;

namespace AterraCore.Boot.Logic.PluginLoading;
// ---------------------------------------------------------------------------------------------------------------------
// Support Code
// ---------------------------------------------------------------------------------------------------------------------
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginLoader(ILogger logger) : IPluginLoader {
    private ILogger Logger { get; } = logger.ForContext<PluginLoader>();
    
    public LinkedList<IPreLoadedPluginDto> Plugins { get; } = [];
    public HashSet<string> Checksums { get; } = [];

    // -----------------------------------------------------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IPluginLoader IterateOverValid(Action<IPluginLoader, IPreLoadedPluginDto> action) {
        foreach (IPreLoadedPluginDto plugin in  GetValidPlugins()) {
            action(this, plugin);
        }
        return this;
    }
    
    
    public IPluginLoader IterateOverValidWithZipImporter(params Action<IPluginLoader, IPreLoadedPluginDto, IPluginZipImporter<PluginConfigXml>>[] actions) {
        foreach (IPreLoadedPluginDto plugin in GetValidPlugins()) {
            using var zipImporter = new PluginZipImporter(plugin.FilePath, logger);
            foreach (Action<IPluginLoader,IPreLoadedPluginDto,IPluginZipImporter<PluginConfigXml>> action in actions) {
                if (plugin.Validity is not PluginValidity.Valid) continue;
                action(this, plugin, zipImporter);
            }
        }
        
        return this;
    }
    public IEnumerable<IPreLoadedPluginDto> GetValidPlugins() => Plugins
        .Where(plugin => plugin.Validity is PluginValidity.Valid);
}
