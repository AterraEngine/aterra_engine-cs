// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.PluginConfig;
using AterraCore.Common.Data;
using AterraCore.Common.Types.FlexiPlug;
using AterraCore.Contracts.Boot.FlexiPlug;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Loggers;
using CodeOfChaos.Extensions;
using CodeOfChaos.Extensions.Serilog;
using System.Reflection;
using Xml.Elements;

namespace AterraCore.Boot.Logic.PluginLoading;
// ---------------------------------------------------------------------------------------------------------------------
// Support Code
// ---------------------------------------------------------------------------------------------------------------------
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginLoader : IPluginLoader {
    private ILogger Logger { get; } = StartupLogger.CreateLogger(false).ForBootOperationContext("PluginLoader");
    
    public LinkedList<IPreLoadedPluginDto> Plugins { get; } = [];
    public HashSet<string> Checksums { get; } = [];

    // -----------------------------------------------------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IPluginLoader IterateOverValid(Action<IPluginLoader, IPreLoadedPluginDto> action) {
        Plugins
            .Where(plugin => plugin.Validity == PluginValidity.Valid)
            .IterateOver(plugin => action(this, plugin));
        return this;
    }
    
    
    public IPluginLoader IterateOverValidWithZipImporter(params Action<IPluginLoader, IPreLoadedPluginDto, IPluginZipImporter<PluginConfigXml>>[] actions) {
        Plugins
            .Where(plugin => plugin.Validity == PluginValidity.Valid)
            .IterateOver(plugin => {
                using var zipImporter = new PluginZipImporter(plugin.FilePath);
                foreach (Action<IPluginLoader,IPreLoadedPluginDto,IPluginZipImporter<PluginConfigXml>> action in actions) {
                    if (plugin.Validity == PluginValidity.Valid) continue;
                    action(this, plugin, zipImporter);
                }
            });
        return this;
    }
}
