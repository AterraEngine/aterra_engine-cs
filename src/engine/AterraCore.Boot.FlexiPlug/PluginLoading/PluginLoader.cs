﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.PluginConfig;
using AterraCore.Common.Data;
using AterraCore.Common.Types.FlexiPlug;
using AterraCore.Contracts.Boot.FlexiPlug;
using AterraCore.Contracts.FlexiPlug;
using Extensions;
using Extensions.Strings;
using Serilog;
using System.Reflection;

namespace AterraCore.Boot.FlexiPlug.PluginLoading;

// ---------------------------------------------------------------------------------------------------------------------
// Support Code
// ---------------------------------------------------------------------------------------------------------------------
public delegate ILoadedPluginDto ZipImportCallback(ILoadedPluginDto pluginData, IPluginZipImporter<PluginConfigXml> zipImporter);

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginLoader(ILogger logger) : IPluginLoader {
    private readonly HashSet<string> _knownHashes = [];

    private ushort _pluginIdCounter;

    private ushort PluginIdCounter {
        get => _pluginIdCounter;
        set {
            // Basically means that the last usable plugin is `FFFE`
            if (value == ushort.MaxValue) {
                string maxId = ushort.MaxValue.ToString("X");
                logger.ExitFatal((int)ExitCodes.PluginIdsExhausted, "Max Plugin Id of {maxId} is exhausted", maxId);
            }
            _pluginIdCounter = value;
        }
    }

    public LinkedList<ILoadedPluginDto> Plugins { get; } = [];

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
        bool trimmedFaulty = TrimFaultyPlugins();

        logger.Information("Total Plugins loaded : {Count}", Plugins.Count);
        return trimmedFaulty && Plugins.Count != 0;
    }

    public void InjectAssemblyAsPlugin(BareAssemblyPlugin manuallyImportedAssembly) {
        // A very special case where the dev wants to assign the current assembly as a plugin
        //      Useful if you don't want to have another project if you just have a single plugin 

        ILoadedPluginDto pluginData = CreateNewPluginDto(manuallyImportedAssembly.Assembly.Location);
        pluginData.Assemblies.Add(manuallyImportedAssembly.Assembly);
        pluginData.Data = new PluginConfigXml {
            ReadableName = manuallyImportedAssembly.ReadableName,
            Author = manuallyImportedAssembly.Author
        };
        pluginData.Validity = PluginValidity.Valid;
        pluginData.IsProcessed = true;

        logger.Debug("{Id} : Custom Assembly assigned ", pluginData.ReadableId);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private ILoadedPluginDto CreateNewPluginDto(string filepath) {
        // PluginCounter exits out of engine as soon as the max PluginId is exhausted
        ILoadedPluginDto pluginData = Plugins.AddLast(new LoadedPluginDto(PluginIdCounter++, filepath)).Value;

        logger.Information("{Id} : Assigned to {Name}", pluginData.ReadableId, pluginData.ReadableName);
        return pluginData;
    }

    private ILoadedPluginDto CheckExists(ILoadedPluginDto pluginData) {
        if (!File.Exists(pluginData.FilePath)) {
            logger.Warning("{Id} : No Zip Found", pluginData.ReadableId);
            return SetInvalid(pluginData);
        }
        logger.Debug("{Id} : Found Zip", pluginData.ReadableId);
        return pluginData;
    }

    private ILoadedPluginDto CheckForDuplicates(ILoadedPluginDto pluginData, IPluginZipImporter<PluginConfigXml> zipImporter) {
        pluginData.CheckSum = zipImporter.CheckSum;

        if (!_knownHashes.Add(pluginData.CheckSum)) {
            logger.Warning("{Id} : Can't assign, duplicate found", pluginData.ReadableId);
            return SetInvalid(pluginData);
        }

        logger.Debug("{Id} : Not a duplicate", pluginData.ReadableId);
        return pluginData;
    }

    private ILoadedPluginDto WithZipImporter(ILoadedPluginDto pluginData, IEnumerable<ZipImportCallback> callbacks) {

        using var zipImporter = new PluginZipImporter(logger, pluginData.FilePath);
        logger.Debug("{Id} : Assigned Zip Importer", pluginData.ReadableId);

        // Check for duplicates here
        //      Easier here because we can use the ZipImporter to confirm hashes.
        //      Use the callbacks like this so zipImport gets disposed off correctly when they are all done
        foreach (ZipImportCallback callback in callbacks) {
            if (IsSkipable(pluginData)) return pluginData;
            callback(pluginData, zipImporter);
        }

        return pluginData;
    }

    private ILoadedPluginDto GetConfigData(ILoadedPluginDto pluginData, IPluginZipImporter<PluginConfigXml> zipImporter) {
        if (!zipImporter.TryGetPluginConfig(out PluginConfigXml? pluginConfigDto)) {
            logger.Warning("{Id} : No valid PluginConfig found. Usually means that the plugin-config.xml was incorrect", pluginData.ReadableId);
            SetInvalid(pluginData);
            return pluginData;
        }

        pluginData.IngestFromPluginConfig(pluginConfigDto);
        logger.Debug("{Id} : Valid PluginConfig found", pluginData.ReadableId);

        return pluginData;
    }

    private ILoadedPluginDto CheckEngineCompatibility(ILoadedPluginDto pluginData, IPluginZipImporter<PluginConfigXml> zipImporter) =>
        // TODO add more checks
        // if ((pluginData.Data?.GameVersion ?? SemanticVersion.Max) > gameVersion) {
        //     logger.Warning("{Plugin} had an incompatible game version of {PluginGameVersion}, compared to the current {GameVersion}", 
        //         pluginData.ReadableName, pluginData.Data?.GameVersion, gameVersion
        //     );
        //     SetInvalid(pluginData);
        //     return;
        // }
        pluginData;

    private ILoadedPluginDto ImportDlls(ILoadedPluginDto pluginData, IPluginZipImporter<PluginConfigXml> zipImporter) {
        if (pluginData.Data == null) return SetInvalid(pluginData);// Shouldn't happen because we import the data up in the chain

        // Extract assembly(s)
        pluginData.Data.Dlls
            .IterateOver(binDto => binDto.FilePath = Path
                .Combine(Paths.Plugins.PluginBinFolder, binDto.FilePath)
                .Replace("\\", "/"));

        logger.Debug("{Id} : Following DLLs defined {@dlls}", pluginData.ReadableId, pluginData.Data?.Dlls.Select(bin => bin.FilePath));

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

    private ILoadedPluginDto SetInvalid(ILoadedPluginDto pluginData) {
        pluginData.Validity = PluginValidity.Invalid;
        logger.Error("{Id} : Set to Invalid", pluginData.ReadableId);

        return pluginData;
    }

    private static bool IsSkipable(ILoadedPluginDto pluginData) =>
        pluginData.IsProcessed
        || pluginData.Validity != PluginValidity.Untested
        && pluginData.Validity == PluginValidity.Invalid;

    private ILoadedPluginDto Validate(ILoadedPluginDto pluginData) {

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

    private ILoadedPluginDto DebugGetAllFiles(ILoadedPluginDto pluginData, IPluginZipImporter<PluginConfigXml> zipImporter) {
        logger.Debug("ALL FILES : {files}", zipImporter.GetFileNamesInZip());
        return pluginData;
    }

    private void DebugPrintAllPlugins() {
        var valuedBuilder = new ValuedStringBuilder();

        Plugins.IterateOver(pluginData => valuedBuilder.AppendLineValued(
        "-plugin ", true, new { pluginData.FilePath, pluginData.Validity })
        );

        logger.Debug(valuedBuilder.ToString(), valuedBuilder.ValuesToArray());
    }


    private bool TrimFaultyPlugins() {
        IEnumerable<ILoadedPluginDto> validPlugins = Plugins.Where(p => p.Validity == PluginValidity.Valid).ToArray();
        if (validPlugins.Count() == Plugins.Count) {
            logger.Debug("Plugins validated correctly");
            return true;
        }

        logger.Warning("Plugins did not get validated correctly, trimming plugin list");

        Plugins
            .Where(p => p.Validity != PluginValidity.Valid)
            .IterateOver(p => Plugins.Remove(p));

        logger.Warning("Plugins list trimmed to to a total of {i} ", Plugins.Count);
        return false;
    }
}
