// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using System.Reflection;
using AterraCore.Common;
using AterraCore.Config.PluginConfig;
using AterraCore.Contracts.Config.Xml;
using AterraCore.Contracts.FlexiPlug;
using Serilog;

namespace AterraCore.FlexiPlug;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class PluginZipImporter(ILogger logger, string zipPath) : IPluginZipImporter<PluginConfigDto>, IDisposable {
    private readonly PluginConfigParser<PluginConfigDto> _pluginConfigParser = new(logger);
    private readonly ZipArchive _archive = ZipFile.OpenRead(zipPath);

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
    public bool TryGetPluginConfig([NotNullWhen(true)] out PluginConfigDto? pluginConfig) {
        pluginConfig = null;
        
        using var memoryStream = new MemoryStream();
        if (!TryGetFileBytesFromZip( Paths.Plugins.PluginConfig, out byte[]? bytes)) {
            logger.Warning("Could not extract {path} from {zip}", Paths.Plugins.PluginConfig, zipPath);
            return false;
        }

        if (!_pluginConfigParser.TrySerializeFromBytes(bytes, out pluginConfig)) {
            logger.Warning("Failed to deserialize plugin config from {path} in {zip}", Paths.Plugins.PluginConfig, zipPath);
            return false;
        }
        
        logger.Information("Plugin Config found within {zip}", zipPath);
        return true;
        
    }

    public bool TryGetDllAssembly(IFileDto binDto, [NotNullWhen(true)] out Assembly? assembly) {
        assembly = null;
        
        try {
            // I have no clue why I need to do this ...
            string filePathFix = binDto.FilePath.Replace("\\", "/");
            
            if (!TryGetFileBytesFromZip(filePathFix, out byte[]? assemblyBytes)) {
                logger.Warning("Could not get bytes for assembly file of {assemblyNameInZip} from {zipPath}", binDto.FilePath, zipPath);
                return false;  
            }
            
            assembly = Assembly.Load(assemblyBytes);
            logger.Information("Assembly file of {assemblyNameInZip} from {zipPath} successfully loaded", binDto.FilePath, zipPath);
            return true;
        }
        catch (Exception e) {
            logger.Warning("Failed to load assembly {assemblyNameInZip} from {zipPath}, {e}", binDto.FilePath, zipPath, e);
            return false;
        }
    }
    
    private bool TryGetFileBytesFromZip(string fileNameInZip, [NotNullWhen(true)] out byte[]? bytes) {
        bytes = null;
        
        ZipArchiveEntry? fileEntry = _archive.GetEntry(fileNameInZip);
        if (fileEntry == null) {
            logger.Warning("File of {fileNameInZip} could not be found in {zipPath}", fileNameInZip, zipPath);
            return false;
        }
        
        using var memoryStream = new MemoryStream();
        using Stream stream = fileEntry.Open();
        stream.CopyTo(memoryStream);
        bytes = memoryStream.ToArray();
        return true;
    }

    public List<string> GetFileNamesInZip() {
        var fileNames = new List<string>();
        try {
            fileNames.AddRange(_archive.Entries.Select(entry => entry.FullName));
        }
        catch(Exception e) {
            logger.Warning("Failed to retrieve file names from {zipPath}, {e}", zipPath, e);
        }
        return fileNames;
    }
}