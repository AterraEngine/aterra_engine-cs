// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Reflection;
using AterraCore.Common;
using AterraCore.Common.FlexiPlug;
using AterraCore.Config.PluginConfig;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.FlexiPlug.Plugin;
using AterraCore.Extensions;
using AterraCore.FlexiPlug.Plugin;
using AterraCore.Loggers;
using Serilog;
using Serilog.Core;

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
    private IPluginDto CreateNewPluginDto(string filepath) {
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
        if (!File.Exists(pluginData.FilePath) ){
            logger.Warning("Plugin did not exist at {filepath}", pluginData.ReadableName);
            return SetInvalid(pluginData);
        }
        logger.Information("Plugin Zip found at {filepath}", pluginData.ReadableName);
        return pluginData;
    }

    private IPluginDto CheckNotDuplicate(IPluginDto pluginData) {

        if (Plugins.All(p => p.Id == pluginData.Id || p.FilePath != pluginData.FilePath)) return pluginData;
        
        logger.Warning("Duplicate plugin found at {filepath}", pluginData.ReadableName);
        return SetInvalid(pluginData);
    }

    private IPluginDto WithZipImporter(IPluginDto pluginData, IEnumerable<ZipImportCallback> callbacks ) {
        
        using var zipImporter = new PluginZipImporter(logger, pluginData.FilePath);
        logger.Information("Assigned new Zip Importer to {plugin}", pluginData.ReadableName);
        
        // Use the callbacks like this so zipImport gets disposed off correctly when they are all done
        foreach (ZipImportCallback callback in callbacks) {
            if (IsSkipable(pluginData)) continue;
            callback(pluginData, zipImporter);
        }

        return pluginData;
    }

    private IPluginDto GetConfigData(IPluginDto pluginData, IPluginZipImporter<PluginConfigDto> zipImporter) {
        if (!zipImporter.TryGetPluginConfig(out PluginConfigDto? pluginConfigDto)) {
            logger.Warning("Could not extract a valid PluginConfigDto from the plugin {plugin}", pluginData.ReadableName);
            SetInvalid(pluginData);
            return pluginData;
        }

        pluginData.IngestFromPluginConfigDto(pluginConfigDto);
        logger.Information("Assigned plugin config of {path} to {name}", pluginData.FilePath, pluginData.ReadableName);
        logger.Information("Plugin {Name} made by {Author}", pluginData.ReadableName, pluginData.Data?.Author);
        
        return pluginData;
    }
    
    private IPluginDto CheckEngineCompatibility(IPluginDto pluginData, IPluginZipImporter<PluginConfigDto> zipImporter) {
        // TODO add more checks
        // if ((pluginData.Data?.GameVersion ?? SemanticVersion.Max) > gameVersion) {
        //     logger.Warning("{Plugin} had an incompatible game version of {PluginGameVersion}, compared to the current {GameVersion}", 
        //         pluginData.ReadableName, pluginData.Data?.GameVersion, gameVersion
        //     );
        //     SetInvalid(pluginData);
        //     return;
        // }
        return pluginData;
    }

    private IPluginDto ImportDlls(IPluginDto pluginData, IPluginZipImporter<PluginConfigDto> zipImporter) {
        if (pluginData.Data == null) return SetInvalid(pluginData); // Shouldn't happen because we import the data up in the chain
        
        // Extract assembly(s)
        pluginData.Data.Dlls
            .IterateOver(binDto => binDto.FilePath = Path
                .Combine(Paths.Plugins.PluginBinFolder, binDto.FileNameInternal)
                .Replace("\\", "/"));
        
        logger.Information("Found the following DLLs for {filepath} : {@dlls}", pluginData.ReadableName, pluginData.Data?.Dlls.Select(bin => bin.FileNameInternal));

        if ((pluginData.Data?.Dlls.Count() ?? 0) == 0) {
            return SetInvalid(pluginData);
        }
        
        pluginData.Assemblies.AddRange(pluginData.Data!.Dlls
            .Select(binDto => {
                if (!zipImporter.TryGetDllAssembly(binDto, out Assembly? assembly)) {
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
    
    private bool IsSkipable(IPluginDto pluginData) {
        bool result = pluginData.IsProcessed 
                      || ( pluginData.Validity != PluginValidity.Untested 
                           && pluginData.Validity == PluginValidity.Invalid);
        if (result) {
            logger.Warning("Skipping {name}", pluginData.ReadableName);
        }
        return result;
    }

    private IPluginDto Validate(IPluginDto pluginData) {
        
        // Todo add more validation steps
        if (pluginData.Data != null && pluginData.Assemblies.Count != pluginData.Data.Dlls.Count()) {
            pluginData.Validity = PluginValidity.Invalid;
            logger.Warning("Amount of assigned DLL's didnt correspond with the loaded assemblies in {plugin}", pluginData.ReadableName);
        }

        if (pluginData.Validity == PluginValidity.Untested) {
            pluginData.Validity = PluginValidity.Valid;
            logger.Information("Plugin {plugin} has been validated correctly", pluginData.ReadableName);
        }

        pluginData.IsProcessed = true;
        return pluginData;
    }

    private IPluginDto DebugGetAllFiles(IPluginDto pluginData, IPluginZipImporter<PluginConfigDto> zipImporter) {
        logger.Debug("ALL FILES : {files}",zipImporter.GetFileNamesInZip());
        return pluginData;
    }

    private void DebugPrintAllPlugins() {
        var valuedBuilder = new ValuedStringBuilder();
        
        Plugins.IterateOver(pluginData => valuedBuilder.AppendLineValued("-plugin ", true, new {pluginData.FilePath, pluginData.Validity}));
        
        logger.Debug(valuedBuilder.ToString(), valuedBuilder.ValuesToArray());
    }
    
    
    private void TrimFaultyPlugins() {
        IEnumerable<IPluginDto> validPlugins = Plugins.Where(p => p.Validity == PluginValidity.Valid).ToArray();
        if (validPlugins.Count() == Plugins.Count) {
            logger.Information("All plugins validated correctly");
            return;
        }
        
        logger.Warning("Some plugins did not get validated correctly, trimming plugin list");
        
        Plugins
            .Where(p => p.Validity != PluginValidity.Valid)
            .ToList() // Can't use the Extensions's ForEach because we are removing certain data
            .ForEach(p => Plugins.Remove(p));
        
        logger.Warning("Plugin list trimmed to to a total of {i} ", Plugins.Count);
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryParseAllPlugins(IEnumerable<string> filePaths) {
        filePaths
            .Select(CreateNewPluginDto)
            .Select(CheckExists).WhereNot(IsSkipable)
            .Select(CheckNotDuplicate).WhereNot(IsSkipable)
            // ZipImporter is disposed after this select statement is completed
            .Select(pluginData => WithZipImporter(pluginData, [
                DebugGetAllFiles,
                GetConfigData,
                CheckEngineCompatibility,
                ImportDlls
            ])).WhereNot(IsSkipable)
            
            // End of the line
            .WhereNot(IsSkipable)
            .IterateOver(Validate);

        DebugPrintAllPlugins();
        TrimFaultyPlugins();
        
        logger.Information("{Count} Plugins have been loaded", Plugins.Count);
        return Plugins.Count != 0;
    }

    public void InjectAssemblyAsPlugin(Assembly assembly) {
        // A very special case where the dev wants to assign the current assembly as a plugin
        //      Useful if you don't want to have another project if you just have a single plugin 
        
        IPluginDto pluginData = CreateNewPluginDto(assembly.Location);
        pluginData.Assemblies.Add(assembly);
        pluginData.Data = new PluginConfigDto {
            ReadableName = "Manually loaded Assembly",
            Author = "Unknown"
        };
        pluginData.Validity = PluginValidity.Valid;
        pluginData.IsProcessed = true;
        
        logger.Information("Current Assembly has been assigned as a Plugin with ID {id}", pluginData.Id);
    }

    public LinkedList<IPlugin> ExportToPlugins() {
        return new LinkedList<IPlugin>(
            Plugins.Select(p => new Plugin.Plugin {
                Id = p.Id,
                ReadableName = p.ReadableName,
                Assemblies = p.Assemblies,
                Types = p.Types
            })
        );
    }
}