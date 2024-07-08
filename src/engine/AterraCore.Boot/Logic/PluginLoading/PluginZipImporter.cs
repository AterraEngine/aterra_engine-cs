// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Common.ConfigFiles.PluginConfig;
using AterraCore.Common.Data;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Loggers;
using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using System.Reflection;
using Xml;
using Xml.Elements;

namespace AterraCore.Boot.Logic.PluginLoading;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginZipImporter(string zipPath) : IPluginZipImporter<PluginConfigXml>, IDisposable {
    private static ILogger Logger { get; } = StartupLogger.CreateLogger(false).ForZipImporterContext();
    
    private readonly ZipArchive _archive = ZipFile.OpenRead(zipPath);
    private readonly XmlParser<PluginConfigXml> _pluginConfigParser = new(Logger, XmlNameSpaces.ConfigPlugin, Paths.Xsd.XsdPluginConfigDto);

    // -----------------------------------------------------------------------------------------------------------------
    // Disposable
    // -----------------------------------------------------------------------------------------------------------------
    public void Dispose() {
        _archive.Dispose();
        GC.SuppressFinalize(this);
    }
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryGetPluginConfig([NotNullWhen(true)] out PluginConfigXml? pluginConfig) {
        pluginConfig = null;

        using var memoryStream = new MemoryStream();
        if (!TryGetFileBytesFromZip(Paths.Plugins.PluginConfig, out byte[]? bytes)) {
            Logger.Warning("Could not extract {path} from {zip}", Paths.Plugins.PluginConfig, zipPath);
            return false;
        }

        if (!_pluginConfigParser.TrySerializeFromBytes(bytes, out pluginConfig)) {
            Logger.Warning("Failed to deserialize plugin config from {path} in {zip}", Paths.Plugins.PluginConfig, zipPath);
            return false;
        }

        Logger.Information("Plugin PluginDtos found within {zip}", zipPath);
        return true;

    }

    public bool TryGetDllAssembly(FileDto binDto, [NotNullWhen(true)] out Assembly? assembly) {
        assembly = null;

        try {
            // I have no clue why I need to do this ...
            string filePathFix = binDto.FilePath.Replace("\\", "/");

            if (!TryGetFileBytesFromZip(filePathFix, out byte[]? assemblyBytes)) {
                Logger.Warning("Could not get bytes for assembly file of {assemblyNameInZip} from {zipPath}", binDto.FilePath, zipPath);
                return false;
            }

            assembly = Assembly.Load(assemblyBytes);
            Logger.Information("Assembly file of {assemblyNameInZip} from {zipPath} successfully loaded", binDto.FilePath, zipPath);
            return true;
        }
        catch (Exception e) {
            Logger.Warning("Failed to load assembly {assemblyNameInZip} from {zipPath}, {e}", binDto.FilePath, zipPath, e);
            return false;
        }
    }

    public List<string> GetFileNamesInZip() {
        var fileNames = new List<string>();
        try {
            fileNames.AddRange(_archive.Entries.Select(entry => entry.FullName));
        }
        catch (Exception e) {
            Logger.Warning("Failed to retrieve file names from {zipPath}, {e}", zipPath, e);
        }
        return fileNames;
    }

    private bool TryGetFileBytesFromZip(string fileNameInZip, [NotNullWhen(true)] out byte[]? bytes) {
        bytes = null;

        ZipArchiveEntry? fileEntry = _archive.GetEntry(fileNameInZip);
        if (fileEntry == null) {
            Logger.Warning("File of {fileNameInZip} could not be found in {zipPath}", fileNameInZip, zipPath);
            return false;
        }

        using var memoryStream = new MemoryStream();
        using Stream stream = fileEntry.Open();
        stream.CopyTo(memoryStream);
        bytes = memoryStream.ToArray();
        return true;
    }


}
