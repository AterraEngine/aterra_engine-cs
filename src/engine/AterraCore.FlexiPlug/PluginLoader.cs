// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Reflection;
using AterraCore.Common;
using AterraCore.Config.PluginConfig;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.FlexiPlug.Plugin;
using AterraCore.FlexiPlug.Plugin;
using Serilog;

namespace AterraCore.FlexiPlug;

// ---------------------------------------------------------------------------------------------------------------------
// Support Code
// ---------------------------------------------------------------------------------------------------------------------
public delegate IPluginData ZipImportCallback(IPluginData pluginData, IPluginZipImporter<PluginConfigDto> zipImporter);

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginLoader(ILogger logger) : IPluginLoader {
    private LinkedList<IPluginData> _plugins { get; set; } = [];
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

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private IPluginData CreateNew(string filepath) {
        IPluginData pluginData = _plugins.AddLast(new PluginData(PluginIdCounter++, filepath)).Value;
        logger.Information("New pluginId of {id} registered for {filepath} ", pluginData.Id, pluginData.ReadableName);
        return pluginData;
    }
    
    private IPluginData CheckExists(IPluginData pluginData) {
        // Quick Exit
        if (IsInvalid(pluginData)) return pluginData;
        
        if (!File.Exists(pluginData.FilePath) ){
            logger.Warning("Plugin did not exist at {filepath}", pluginData.ReadableName);
            return SetInvalid(pluginData);
        }
        logger.Information("Plugin Zip found at {filepath}", pluginData.ReadableName);
        return pluginData;
    }

    private IPluginData WithZipImporter(IPluginData pluginData, IEnumerable<ZipImportCallback> callbacks ) {
        // Quick Exit
        if (IsInvalid(pluginData)) return pluginData;
        
        using var zipImporter = new PluginZipImporter(logger, pluginData.FilePath);
        logger.Information("Assigned new Zip Importer to {plugin}", pluginData.ReadableName);
        
        foreach (ZipImportCallback callback in callbacks) {
            callback(pluginData, zipImporter);
        }

        return pluginData;
    }

    private IPluginData GetConfigData(IPluginData pluginData, IPluginZipImporter<PluginConfigDto> zipImporter) {
        // Quick Exit
        if (IsInvalid(pluginData)) return pluginData;
        
        if (!zipImporter.TryGetPluginConfig(out PluginConfigDto? pluginConfigDto)) {
            logger.Warning("Could not extract a valid PluginConfigDto from the plugin {plugin}", pluginData.ReadableName);
            return SetInvalid(pluginData);
        }

        pluginData.IngestFromPluginConfigDto(pluginConfigDto);
        logger.Information("Assigned plugin config of {path} to {name}", pluginData.FilePath, pluginData.ReadableName);
        logger.Information("Plugin {Name} made by {Author}", pluginData.ReadableName, pluginData.Data?.Author);

        return pluginData;
    }
    
    private IPluginData CheckEngineCompatibility(IPluginData pluginData, IPluginZipImporter<PluginConfigDto> zipImporter) {
        // Quick Exit
        if (IsInvalid(pluginData)) return pluginData;
        
        // TODO CHECK FOR ENGINE COMPATIBILITY
        logger.Warning("Skipping engine compatibility check");
        return pluginData;
    }

    private IPluginData ImportDlls(IPluginData pluginData, IPluginZipImporter<PluginConfigDto> zipImporter) {
        // Quick Exit
        if (IsInvalid(pluginData)) return pluginData;
        if (pluginData.Data == null) return pluginData; // Shouldn't happen because we import the data up in the chain
        
        // Extract assembly(s)
        pluginData.Data.Dlls = pluginData.Data.Dlls.Select(f => Path.Combine(Paths.Plugins.PluginBinFolder, f).Replace("\\", "/")).ToList();
        logger.Information("Found the following DLLs for {filepath} : {dlls}", pluginData.ReadableName, pluginData.Data?.Dlls);

        if ((pluginData.Data?.Dlls.Count ?? 0) == 0) return SetInvalid(pluginData);
        
        pluginData.Assemblies.AddRange(pluginData.Data!.Dlls
            .Select(dll => {
                if (!zipImporter.TryGetPluginAssembly(dll, out Assembly? assembly)) {
                    logger.Warning("Could not load plugin assembly for {filepath}", pluginData.ReadableName);
                    SetInvalid(pluginData);
                    return assembly;
                }

                logger.Information("Found assembly for {filepath} as {assembly}", pluginData.FilePath, assembly.FullName);
                return assembly;
            })
            .Where(assembly => assembly != null)
            .Select(assembly => assembly!) 
        );
        
        return pluginData;
    }
    
    private static IPluginData SetInvalid(IPluginData pluginData) {
        pluginData.Validity = PluginValidity.Invalid;
        return pluginData;
    }
    
    private static bool IsInvalid(IPluginData pluginData) => pluginData.Validity == PluginValidity.Invalid;

    private IPluginData Validate(IPluginData pluginData) {
        // Quick Exit
        if (IsInvalid(pluginData)) return pluginData;
        
        // Todo add more validation steps
        if (pluginData.Validity == PluginValidity.Untested) pluginData.Validity = PluginValidity.Valid;
        return pluginData;
    }

    private IPluginData CheckValidity(IPluginData pluginData) {
        // Check validity of all and log accordingly
        bool validity = _plugins.All(p => p.Validity == PluginValidity.Valid);
        if (validity) {
            logger.Information("All Plugins validated successfully");
        } else {
            logger.Error(
                "Plugins could not be loaded correctly. The following is a full list of which aren't valid {l}",
                _plugins
                    .Where(p => p.Validity != PluginValidity.Valid)
                    .Select(p => new { p.Id, p.FilePath })
            );
        }

        return pluginData;
    }

    private IPluginData DebugGetAllFiles(IPluginData pluginData, IPluginZipImporter<PluginConfigDto> zipImporter) {
        logger.Debug("ALL FILES : {files}",zipImporter.GetFileNamesInZip());
        return pluginData;
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryParseAllPlugins(IEnumerable<string> filePaths) {
        IEnumerable<IPluginData> plugins = filePaths
            .Select(CreateNew)
            .Select(CheckExists)
            .Select(pluginData => WithZipImporter(pluginData, [
                GetConfigData,
                CheckEngineCompatibility,
                ImportDlls
            ]))
            .Select(Validate)
            .Select(CheckValidity)
            
            // Filter those plugins which did not get validated correctly
            .Where(p => !IsInvalid(p));
        
        IEnumerable<IPluginData> pluginDatas = plugins as IPluginData[] ?? plugins.ToArray();
        
        if (pluginDatas.Count() != Plugins.Count) {
            logger.Warning("Some plugins did not get validated correctly, trimming invalid");
            _plugins = new LinkedList<IPluginData>(pluginDatas.Where(IsInvalid));
        }
        
        return Plugins.Count != 0;
        
    }
}