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
    public LinkedList<IPluginData> Plugins { get; private set; } = [];

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
        IPluginData pluginData;
        
        try {
            pluginData = Plugins.AddLast(new PluginData(PluginIdCounter++, filepath)).Value;
        }
        catch (OverflowException e) {
            logger.Error(e, "PluginIdCounter for {Plugin} overflowed", filepath);
            throw; // one of the few actual exceptions which should occur during startup
        }
        
        logger.Information("New pluginId of {id} registered for {filepath} ", pluginData.Id, pluginData.ReadableName);
        return pluginData;
    }
    
    private IPluginData CheckExists(IPluginData pluginData) {
        // Quick Exit
        if (IsSkipable(pluginData)) return pluginData;
        
        if (!File.Exists(pluginData.FilePath) ){
            logger.Warning("Plugin did not exist at {filepath}", pluginData.ReadableName);
            return SetInvalid(pluginData);
        }
        logger.Information("Plugin Zip found at {filepath}", pluginData.ReadableName);
        return pluginData;
    }

    private IPluginData WithZipImporter(IPluginData pluginData, IEnumerable<ZipImportCallback> callbacks ) {
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

    private IPluginData GetConfigData(IPluginData pluginData, IPluginZipImporter<PluginConfigDto> zipImporter) {
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
    
    private IPluginData CheckEngineCompatibility(IPluginData pluginData, IPluginZipImporter<PluginConfigDto> zipImporter) {
        // Quick Exit
        if (IsSkipable(pluginData)) return pluginData;
        
        // TODO CHECK FOR ENGINE COMPATIBILITY
        logger.Warning("Skipping engine compatibility check");
        return pluginData;
    }

    private IPluginData ImportDlls(IPluginData pluginData, IPluginZipImporter<PluginConfigDto> zipImporter) {
        // Quick Exit
        if (IsSkipable(pluginData)) return pluginData;
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
    
    private static bool IsSkipable(IPluginData pluginData) => 
        pluginData.Validity == PluginValidity.Invalid
        || pluginData.IsProcessed;

    private void Validate(IPluginData pluginData) {
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

    private IPluginData DebugGetAllFiles(IPluginData pluginData, IPluginZipImporter<PluginConfigDto> zipImporter) {
        logger.Debug("ALL FILES : {files}",zipImporter.GetFileNamesInZip());
        return pluginData;
    }
    
    private void TrimFaultyPlugins(int originalLength) {
        // Warn This is dirty, but will work for now.
        IEnumerable<IPluginData> plugins = Plugins.Where(p => p.Validity == PluginValidity.Valid);
        
        // Need to check if there is an entry assembly assigned as a plugin as well
        IPluginData? entryAssemblyPlugin = Plugins.FirstOrDefault(p => p?.FilePath == Assembly.GetEntryAssembly()?.Location, null);
        int offset = entryAssemblyPlugin != null ? 1 : 0; // else if check won't work as expected if there is a entry assembly as plugin.
        
        if (plugins.Count() == originalLength  + offset) {
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
        
        TrimFaultyPlugins(paths.Length);
        
        logger.Information("{Count} Plugins have been loaded", Plugins.Count);
        return Plugins.Count != 0;
    }

    public void InjectCurrentAssemblyAsPlugin(Assembly assembly) {
        IPluginData pluginData = CreateNew(assembly.Location);
        pluginData.Assemblies.Add(assembly);
        pluginData.Data = new PluginConfigDto {
            ReadableName = "Starting Assembly"
        };
        pluginData.Validity = PluginValidity.Valid;
        pluginData.IsProcessed = true;
        
        logger.Information("Current Assembly has been assigned as a Plugin with ID {id}", pluginData.Id);
    }
}