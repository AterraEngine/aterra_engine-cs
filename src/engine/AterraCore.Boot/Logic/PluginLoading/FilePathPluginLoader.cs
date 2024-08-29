// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles;
using AterraCore.Contracts.Boot.Logic.PluginLoading.Dto;
using AterraCore.Contracts.FlexiPlug;

namespace AterraCore.Boot.Logic.PluginLoading;

// ---------------------------------------------------------------------------------------------------------------------
// Support Code
// ---------------------------------------------------------------------------------------------------------------------
using ZipImporterAction=Action<IFilePathPluginLoader, IPluginBootDto, IPluginZipImporter<PluginConfigXml>>;
using PluginAction=Action<IFilePathPluginLoader, IRawPluginBootDto, IPluginBootDto>;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class FilePathPluginLoader(ILogger logger) : IFilePathPluginLoader {
    private ILogger Logger { get; } = logger.ForContext<FilePathPluginLoader>();
    public LinkedList<(IRawPluginBootDto rawPluginBootDto, IPluginBootDto pluginBootDto)> Plugins { get; } = [];
    public HashSet<string> Checksums { get; } = [];

    // -----------------------------------------------------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IFilePathPluginLoader IterateOverValid(PluginAction action) {
        foreach ((IRawPluginBootDto rawPluginBootDto, IPluginBootDto pluginBootDto) in GetValidPlugins()) action(this, rawPluginBootDto, pluginBootDto);
        return this;
    }


    public IFilePathPluginLoader IterateOverValidWithZipImporter(params ZipImporterAction[] actions) {
        foreach ((IRawPluginBootDto rawPluginBootDto, IPluginBootDto pluginBootDto) in GetValidPlugins()) {
            using var zipImporter = new PluginZipImporter(rawPluginBootDto.FilePath, logger);
            foreach (ZipImporterAction action in actions) {
                action(this, pluginBootDto, zipImporter);
                // Break away from the zip importer if plugin became invalid
                if (!pluginBootDto.IsValid) break;
            }
        }

        return this;
    }
    public IEnumerable<(IRawPluginBootDto rawPluginBootDto, IPluginBootDto pluginBootDto)> GetValidPlugins() =>
        Plugins.Where(plugin => plugin.pluginBootDto.IsValid);
}
