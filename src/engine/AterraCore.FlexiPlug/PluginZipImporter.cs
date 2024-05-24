// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using AterraCore.Common;
using AterraCore.Common.Config;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.FlexiPlug.Config;
using Serilog;
using Xml;
using Xml.Elements;
namespace AterraCore.FlexiPlug;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PluginZipImporter(ILogger logger, string zipPath) : IPluginZipImporter<PluginConfigXml>, IDisposable {
    private readonly ZipArchive _archive = ZipFile.OpenRead(zipPath);
    private readonly ConfigXmlParser<PluginConfigXml> _pluginConfigParser = new(logger, XmlNameSpaces.ConfigPlugin, Paths.Xsd.XsdPluginConfigDto);
    private string? _checkSum;

    // -----------------------------------------------------------------------------------------------------------------
    // Disposable
    // -----------------------------------------------------------------------------------------------------------------
    public void Dispose() {
        _archive.Dispose();
        GC.SuppressFinalize(this);
    }
    public string CheckSum => _checkSum ??= ComputeSha256Hash();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryGetPluginConfig([NotNullWhen(true)] out PluginConfigXml? pluginConfig) {
        pluginConfig = null;

        using var memoryStream = new MemoryStream();
        if (!TryGetFileBytesFromZip(Paths.Plugins.PluginConfig, out byte[]? bytes)) {
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

    public List<string> GetFileNamesInZip() {
        var fileNames = new List<string>();
        try {
            fileNames.AddRange(_archive.Entries.Select(entry => entry.FullName));
        }
        catch (Exception e) {
            logger.Warning("Failed to retrieve file names from {zipPath}, {e}", zipPath, e);
        }
        return fileNames;
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

    private string ComputeSha256Hash() {
        using var mySha256 = SHA256.Create();
        using FileStream stream = File.OpenRead(zipPath);
        stream.Position = 0;
        byte[] hashBytes = mySha256.ComputeHash(stream);
        var builder = new StringBuilder();
        foreach (byte b in hashBytes) {
            builder.Append(b.ToString("x2"));
        }

        return builder.ToString();
    }
}