﻿// ---------------------------------------------------------------------------------------------------------------------
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

    private ExternalPluginImporter _pluginImporter = new(logger);
    
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
            
            logger.Debug("{filepath} contains {paths}", pluginData.FilePath, _pluginImporter.GetFileNamesInZip(pluginData.FilePath));

            if (!_pluginImporter.TryGetPluginConfig(pluginData.FilePath, out PluginConfigDto? pluginConfigDto)) {
                logger.Warning("Could not extract a valid PluginConfigDto from the plugin {plugin}", pluginData.FilePath);
                pluginData.Validity = PluginValidity.Invalid;
                continue;
            }
            logger.Information("Plugin Config correct found for {filepath}", pluginData.FilePath);

            // TODO CHECK FOR ENGINE COMPATIBILITY
            logger.Warning("Skipping engine compatibility check");
            
            // Extract assembly(s)
            pluginData.Dlls = pluginConfigDto.Dlls.Select(f => Path.Combine(Paths.PluginDlls, f)).ToArray();
            if (!_pluginImporter.TryGetPluginAssembly(pluginData.FilePath, pluginData.Dlls[0], out Assembly? assembly)) {
                logger.Warning("Could not load plugin assembly for {filepath}", pluginData.FilePath);
                pluginData.Validity = PluginValidity.Invalid;
                continue;
            }
            pluginData.Assembly = assembly;
            
            pluginData.Validity = PluginValidity.Valid;
        }

        return _plugins.All(p => p.Validity == PluginValidity.Valid);
    }
    
    public IEnumerable<string> FindPluginDlls(string folderPath) {
        try {
            // Grab all DLL files from the folder
            return Directory.GetFiles(folderPath, "*.dll", SearchOption.AllDirectories);
            // .Select(Assembly.LoadFrom)
            // .Select(assembly => new PluginData(PluginIdCounter++, assembly)).ToList()
            // .ForEach(data => _plugins.AddLast(data));
        }
        
        catch(Exception e) {
            logger.Error(e, "An error occurred while trying to find plugin DLLs"); // todo rewrite so we try and log the error in the LINQ?
            throw;
        }
    }
}