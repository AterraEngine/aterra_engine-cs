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
public delegate IPluginDto ZipImportCallback(IPluginDto pluginData, IPluginZipImporter<PluginConfigDto> zipImporter);

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginLoader(ILogger logger) : IPluginLoader {
    public LinkedList<IPluginDto> Plugins { get; } = [];

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
    private IPluginDto CreateNew(string filepath) {
        IPluginDto pluginData;
        
        try {
            pluginData = Plugins.AddLast(new PluginDto(PluginIdCounter++, filepath)).Value;
        }
        catch (Exception e) {
            logger.Error(e, "Unexpected exception when creating a new plugin {Plugin}: {Exception}", filepath, e.Message);
            throw;
        }
        
        logger.Information("New pluginId of {id} registered for {filepath} ", pluginData.Id, pluginData.ReadableName);
        return pluginData;
    }
    
    private IPluginDto CheckExists(IPluginDto pluginData) {
        // Quick Exit
        if (IsSkipable(pluginData)) return pluginData;
        
        if (!File.Exists(pluginData.FilePath) ){
            logger.Warning("Plugin did not exist at {filepath}", pluginData.ReadableName);
            return SetInvalid(pluginData);
        }
        logger.Information("Plugin Zip found at {filepath}", pluginData.ReadableName);
        return pluginData;
    }

    private IPluginDto WithZipImporter(IPluginDto pluginData, IEnumerable<ZipImportCallback> callbacks ) {
        // Quick Exit
        if (IsSkipable(pluginData)) return pluginData;
        
        using var zipImporter = new PluginZipImporter(logger, pluginData.FilePath);
        logger.Information("Assigned new Zip Importer to {plugin}", pluginData.ReadableName);
        
        // Use the callbacks like this so zipImport gets disposed off correctly when they are all done
        foreach (ZipImportCallback callback in callbacks) {
            callback(pluginData, zipImporter);
        }

        return pluginData;
    }

    private IPluginDto GetConfigData(IPluginDto pluginData, IPluginZipImporter<PluginConfigDto> zipImporter) {
        // Quick Exit
        if (IsSkipable(pluginData)) return pluginData;
        
        if (!zipImporter.TryGetPluginConfig(out PluginConfigDto? pluginConfigDto)) {
            logger.Warning("Could not extract a valid PluginConfigDto from the plugin {plugin}", pluginData.ReadableName);
            return SetInvalid(pluginData);
        }

        pluginData.IngestFromPluginConfigDto(pluginConfigDto);
        logger.Information("Assigned plugin config of {path} to {name}", pluginData.FilePath, pluginData.ReadableName);
        logger.Information("Plugin {Name} made by {Author}", pluginData.ReadableName, pluginData.Data?.Author);

        return pluginData;
    }
    
    private IPluginDto CheckEngineCompatibility(IPluginDto pluginData, IPluginZipImporter<PluginConfigDto> zipImporter) {
        // Quick Exit
        if (IsSkipable(pluginData)) return pluginData;

        // TODO add more checks
        // if ((pluginData.Data?.GameVersion ?? SemanticVersion.Max) > gameVersion) {
        //     logger.Warning("{Plugin} had an incompatible game version of {PluginGameVersion}, compared to the current {GameVersion}", 
        //         pluginData.ReadableName, pluginData.Data?.GameVersion, gameVersion
        //     );
        //     return SetInvalid(pluginData);
        // }
        
        return pluginData;
    }

    private IPluginDto ImportDlls(IPluginDto pluginData, IPluginZipImporter<PluginConfigDto> zipImporter) {
        // Quick Exit
        if (IsSkipable(pluginData)) return pluginData;
        if (pluginData.Data == null) return pluginData; // Shouldn't happen because we import the data up in the chain
        
        // Extract assembly(s)
        pluginData.Data.Dlls = pluginData.Data.Dlls.Select(f => Path.Combine(Paths.Plugins.PluginBinFolder, f).Replace("\\", "/")).ToList();
        logger.Information("Found the following DLLs for {filepath} : {dlls}", pluginData.ReadableName, pluginData.Data?.Dlls);

        if ((pluginData.Data?.Dlls.Count ?? 0) == 0) return SetInvalid(pluginData);
        
        pluginData.Assemblies.AddRange(pluginData.Data!.Dlls
            .Select(dll => {
                if (!zipImporter.TryGetDllAssembly(dll, out Assembly? assembly)) {
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
    
    private IPluginDto SetInvalid(IPluginDto pluginData) {
        pluginData.Validity = PluginValidity.Invalid;
        logger.Error("{Plugin} set to Invalid", pluginData.ReadableName);
        return pluginData;
    }
    
    private static bool IsSkipable(IPluginDto pluginData) => 
        pluginData.Validity == PluginValidity.Invalid
        || pluginData.IsProcessed;

    private void Validate(IPluginDto pluginData) {
        // Quick Exit
        if (IsSkipable(pluginData)) return;
        
        // Todo add more validation steps
        if (pluginData.Data != null && pluginData.Assemblies.Count != pluginData.Data.Dlls.Count) {
            pluginData.Validity = PluginValidity.Invalid;
            logger.Warning("Amount of assigned DLL's didnt correspond with the loaded assemblies in {plugin}", pluginData.ReadableName);
        }

        if (pluginData.Validity == PluginValidity.Untested) {
            pluginData.Validity = PluginValidity.Valid;
            logger.Information("Plugin {plugin} has been validated correctly", pluginData.ReadableName);
        }

        pluginData.IsProcessed = true;
    }

    private IPluginDto DebugGetAllFiles(IPluginDto pluginData, IPluginZipImporter<PluginConfigDto> zipImporter) {
        logger.Debug("ALL FILES : {files}",zipImporter.GetFileNamesInZip());
        return pluginData;
    }
    
    private void TrimFaultyPlugins() {
        // Warn This is dirty, but will work for now.
        IEnumerable<IPluginDto> validPlugins = Plugins.Where(p => p.Validity == PluginValidity.Valid);
        
        if (validPlugins.Count() == Plugins.Count) {
            logger.Information("All plugins validated correctly");
            return;
        }
        
        logger.Warning("Some plugins did not get validated correctly, trimming plugin list");
        Plugins
            .Where(p => p.Validity == PluginValidity.Valid)
            .ToList()
            .ForEach(p => Plugins.Remove(p));
        logger.Warning("Plugin list trimmed to to a total of {i} ", Plugins.Count);
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryParseAllPlugins(IEnumerable<string> filePaths) {
        string[] paths = filePaths.ToArray();
        paths
            .ToList()
            .Select(CreateNew)
            .Select(CheckExists)
            // ZipImporter is disposed after this select statement is completed
            .Select(pluginData => WithZipImporter(pluginData, [
                DebugGetAllFiles, // Todo remove this in production
                GetConfigData,
                CheckEngineCompatibility,
                ImportDlls
            ]))
            
            // End of the line
            .ToList()
            .ForEach(Validate);
        
        TrimFaultyPlugins();
        
        logger.Information("{Count} Plugins have been loaded", Plugins.Count);
        return Plugins.Count != 0;
    }

    public void InjectAssemblyAsPlugin(Assembly assembly) {
        // A very special case where the dev wants to assign the current assembly as a plugin
        //      Useful if you don't want to have another project if you just have a single plugin 
        
        IPluginDto pluginData = CreateNew(assembly.Location);
        pluginData.Assemblies.Add(assembly);
        pluginData.Data = new PluginConfigDto {
            ReadableName = "Starting Assembly",
            Author = "Unknown"
        };
        pluginData.Validity = PluginValidity.Valid;
        pluginData.IsProcessed = true;
        
        logger.Information("Current Assembly has been assigned as a Plugin with ID {id}", pluginData.Id);
    }
}