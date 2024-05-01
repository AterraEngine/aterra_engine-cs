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
using AterraCore.Loggers.Helpers;
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
    private HashSet<string> _knownHashes = new();


    private ushort _pluginIdCounter;
    private ushort PluginIdCounter {
        get => _pluginIdCounter;
        set {
            if (value == ushort.MaxValue) {
                var maxId = ushort.MaxValue.ToString("X");
                logger.ExitFatal(ExitCodes.PluginIdsExhausted, "Max Plugin Id of {maxId} is exhausted",maxId);
            }
            _pluginIdCounter = value;
        }
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private IPluginDto CreateNewPluginDto(string filepath) {
        // PluginCounter exits out of engine as soon as the max PluginId is exhausted
        IPluginDto pluginData = Plugins.AddLast(new PluginDto(PluginIdCounter++, filepath)).Value;
        
        logger.Debug("{Id} : Assigned to {Name}", pluginData.ReadableId, pluginData.ReadableName);
        return pluginData;
    }
    
    private IPluginDto CheckExists(IPluginDto pluginData) {
        if (!File.Exists(pluginData.FilePath) ){
            logger.Warning("{Id} : No Zip Found", pluginData.ReadableId);
            return SetInvalid(pluginData);
        }
        logger.Debug("{Id} : Found Zip", pluginData.ReadableId);
        return pluginData;
    }

    private IPluginDto CheckForDuplicates(IPluginDto pluginData, IPluginZipImporter<PluginConfigDto> zipImporter) {
        pluginData.CheckSum = zipImporter.CheckSum;
        
        if (!_knownHashes.Add(pluginData.CheckSum)) {
            logger.Warning("{Id} : Can't assign, duplicate found", pluginData.ReadableId);
            return SetInvalid(pluginData);
        }

        logger.Debug("{Id} : Not a duplicate", pluginData.ReadableId);
        return pluginData;
    }

    private IPluginDto WithZipImporter(IPluginDto pluginData, IEnumerable<ZipImportCallback> callbacks ) {
        
        using var zipImporter = new PluginZipImporter(logger, pluginData.FilePath);
        logger.Debug("{Id} : Assigned Zip Importer", pluginData.ReadableId);
        
        // Check for duplicates here
        //      Easier here because we can use the ZipImporter to confirm hashes.
        // Use the callbacks like this so zipImport gets disposed off correctly when they are all done
        foreach (ZipImportCallback callback in callbacks) {
            if (IsSkipable(pluginData)) return pluginData;
            callback(pluginData, zipImporter);
        }

        return pluginData;
    }

    private IPluginDto GetConfigData(IPluginDto pluginData, IPluginZipImporter<PluginConfigDto> zipImporter) {
        if (!zipImporter.TryGetPluginConfig(out PluginConfigDto? pluginConfigDto)) {
            logger.Warning("{Id} : No valid PluginConfig found. Usually means that the plugin-config.xml was incorrect", pluginData.ReadableId);
            SetInvalid(pluginData);
            return pluginData;
        }

        pluginData.IngestFromPluginConfigDto(pluginConfigDto);
        logger.Debug("{Id} : Valid PluginConfig found", pluginData.ReadableId);
        
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
        
        logger.Debug("{Id} : Following DLLs defined {@dlls}", pluginData.ReadableId, pluginData.Data?.Dlls.Select(bin => bin.FileNameInternal));

        if ((pluginData.Data?.Dlls.Count() ?? 0) == 0) {
            return SetInvalid(pluginData);
        }
        
        pluginData.Assemblies.AddRange(pluginData.Data!.Dlls
            .Select(binDto => {
                if (!zipImporter.TryGetDllAssembly(binDto, out Assembly? assembly)) {
                    logger.Warning("{Id} : Assembly could not be loaded from {Path}", pluginData.ReadableId, binDto.FilePath);
                    SetInvalid(pluginData);
                    return assembly;
                }

                logger.Debug("{Id} : Assembly {assembly} loaded", pluginData.ReadableId, assembly.FullName);
                return assembly;
            })
            .Where(assembly => assembly != null)
            .Select(assembly => assembly!) 
        );

        return pluginData;
    }
    
    private IPluginDto SetInvalid(IPluginDto pluginData) {
        pluginData.Validity = PluginValidity.Invalid;
        logger.Error("{Id} : Set to Invalid", pluginData.ReadableId);

        return pluginData;
    }
    
    private static bool IsSkipable(IPluginDto pluginData) {
        return pluginData.IsProcessed 
            || ( pluginData.Validity != PluginValidity.Untested 
                 && pluginData.Validity == PluginValidity.Invalid);
    }

    private IPluginDto Validate(IPluginDto pluginData) {
            
        // Todo add more validation steps
        if (pluginData.Data != null && pluginData.Assemblies.Count != pluginData.Data.Dlls.Count()) {
            pluginData.Validity = PluginValidity.Invalid;
            logger.Warning("{Id} : Amount of assigned DLL's didnt correspond with its loaded assemblies", pluginData.ReadableId);
        }

        if (pluginData.Validity == PluginValidity.Untested) {
            pluginData.Validity = PluginValidity.Valid;
            logger.Debug("{Id} : Validated correctly", pluginData.ReadableId);
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
        
        Plugins.IterateOver(pluginData => valuedBuilder.AppendLineValued(
            "-plugin ", true, new {pluginData.FilePath, pluginData.Validity})
        );
        
        logger.Debug(valuedBuilder.ToString(), valuedBuilder.ValuesToArray());
    }
    
    
    private void TrimFaultyPlugins() {
        IEnumerable<IPluginDto> validPlugins = Plugins.Where(p => p.Validity == PluginValidity.Valid).ToArray();
        if (validPlugins.Count() == Plugins.Count) {
            logger.Debug("Plugins validated correctly");
            return;
        }
        
        logger.Warning("Plugins did not get validated correctly, trimming plugin list");
        
        Plugins
            .Where(p => p.Validity != PluginValidity.Valid)
            .ToList() // Can't use the Extensions's ForEach because we are removing certain data
            .ForEach(p => Plugins.Remove(p));
        
        logger.Warning("Plugins list trimmed to to a total of {i} ", Plugins.Count);
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryParseAllPlugins(IEnumerable<string> filePaths) {
        filePaths
            .Select(CreateNewPluginDto)
            .Select(CheckExists).WhereNot(IsSkipable)
            
            // ZipImporter is disposed after this select statement is completed
            .Select(pluginData => WithZipImporter(pluginData, [
                CheckForDuplicates,
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
        
        logger.Information("Total Plugins loaded : {Count}", Plugins.Count);
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
        
        logger.Debug("{Id} : Custom Assembly assigned ", pluginData.ReadableId);
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