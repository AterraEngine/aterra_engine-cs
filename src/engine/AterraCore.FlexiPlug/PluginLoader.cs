// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using AterraCore.Common;
using AterraCore.Config.PluginConfig;
using AterraCore.Contracts.FlexiPlug.Plugin;
using AterraCore.FlexiPlug.Plugin;
using Serilog;

namespace AterraCore.FlexiPlug;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginLoader(ILogger logger) {
    private readonly LinkedList<IPluginData> _plugins = [];
    public LinkedList<IPluginData> Plugins => _plugins;

    private ushort _pluginIdCounter;
    private ushort PluginIdCounter {
        get => _pluginIdCounter;
        set {
            if (value == ushort.MaxValue) {
                var maxId = ushort.MaxValue.ToString("X");
                logger.Fatal("Max Plugin Id of {maxId} is exhausted",maxId);
                throw new OverflowException($"Max Plugin Id of {maxId} is exhausted");
            }
            _pluginIdCounter = value;
        }
    }

    // private ExternalPluginImporter _pluginImporter = new(logger);
    private ExternalPluginImporter GetNewPluginImporter(string zipPath) =>  new(logger, zipPath);
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryParseAllPlugins(IEnumerable<string> filePaths) {
        foreach (string filePath in filePaths) {
            var pluginData = new PluginData(PluginIdCounter++, filePath);
            _plugins.AddLast(pluginData);
            logger.Information("New pluginId of {id} registered for {filepath} ", pluginData.Id, pluginData.FilePath);
            
            if (!File.Exists(pluginData.FilePath) ){
                logger.Warning("Plugin did not exist at {filepath}", pluginData.FilePath);
                pluginData.Validity = PluginValidity.Invalid;
                continue;
            }
            logger.Information("Plugin Zip found at {filepath}", pluginData.FilePath);
            
            // logger.Debug("{filepath} contains {paths}", pluginData.FilePath, _pluginImporter.GetFileNamesInZip(pluginData.FilePath));
            using ExternalPluginImporter pluginImporter = GetNewPluginImporter(filePath);
            
            if (!pluginImporter.TryGetPluginConfig(out PluginConfigDto? pluginConfigDto)) {
                logger.Warning("Could not extract a valid PluginConfigDto from the plugin {plugin}", pluginData.FilePath);
                pluginData.Validity = PluginValidity.Invalid;
                continue;
            }
            logger.Information("Plugin Config correctly found for {Name} by {Author}", pluginConfigDto.ReadableName, pluginConfigDto.Author);

            // TODO CHECK FOR ENGINE COMPATIBILITY
            logger.Warning("Skipping engine compatibility check");
            
            // Extract assembly(s)
            // ToDo import more than one assembly
            pluginData.Dlls = pluginConfigDto.Dlls.Select(f => Path.Combine(Paths.Plugins.PluginBinFolder, f)).ToArray();
            if (!pluginImporter.TryGetPluginAssembly(pluginData.Dlls[0], out Assembly? assembly)) {
                logger.Warning("Could not load plugin assembly for {filepath}", pluginData.FilePath);
                pluginData.Validity = PluginValidity.Invalid;
                continue;
            }
            pluginData.Assembly = assembly;
            
            pluginData.Validity = PluginValidity.Valid;
        }
        
        // Check validity of all and log accordingly
        bool validity = _plugins.All(p => p.Validity == PluginValidity.Valid);
        if (!validity) {
            logger.Error(
                "Plugins could not be loaded correctly. The following is a full list of which aren't valid {l}",
                _plugins
                    .Where(p => p.Validity != PluginValidity.Valid)
                    .Select(p => new {p.Id, p.FilePath})
            );
        }
        
        logger.Information("All Plugins validated successfully");
        return validity;
    }
}