// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using AterraCore.Common;
using AterraCore.Config.Xml;
using Microsoft.Build.Evaluation;
using Serilog;

namespace AterraCore.FlexiPlug;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class ExtrenalPluginImporter(ILogger logger) {
    private Assembly? _loadPluginCsProj(PluginRecordDto pluginDataDto) {
        string[] csprojFiles = Directory.GetFiles(pluginDataDto.FilePath, "*.csproj");
        if (csprojFiles.Length == 0) {
            logger.Warning("No .csproj found at the assigned folder path of {FolderPath}", pluginDataDto.FilePath);
            return null;
        }
        
        // Extract the directory path containing the .csproj file
        string? projectFolderPath = Path.GetDirectoryName(csprojFiles[0]);
        if (projectFolderPath is null) {
            logger.Error("A .csproj was found for the plugin, but DirectoryName could not be retrieved from {FolderPath}", pluginDataDto.FilePath);
            return null;
        }
        
        // Configure the project collection for the build
        var projectCollection = new ProjectCollection();
        projectCollection.DefaultToolsVersion = "17.9.8";
        
        // Load the project into the project collection
        Project project = projectCollection.LoadProject(csprojFiles[0]);
        
        // Build the project and get the output assembly path
        project.Build(); // warn ? keep this in?
        string outputAssemblyPath = Path.Combine(projectFolderPath, project.GetPropertyValue("OutputPath"), project.GetPropertyValue("AssemblyName") + ".dll");

        // Load the output assembly
        try {
            Assembly assembly = Assembly.LoadFrom(outputAssemblyPath);
            return assembly;
        }
        catch (Exception e) {
            logger.Fatal("Assembly could not be loaded from {a}", outputAssemblyPath );
            throw;
        }
        
    }
    
    private bool TryGetFileBytesFromZip(string zipPath, string fileNameInZip, [NotNullWhen(true)] out byte[]? bytes) {
        bytes = null;
        
        using ZipArchive archive = ZipFile.OpenRead(zipPath);
        ZipArchiveEntry? fileEntry = archive.GetEntry(fileNameInZip);
        if (fileEntry == null) return false;
        
        using Stream stream = fileEntry.Open();
        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        bytes = ms.ToArray();
        return true;
    }
    private Assembly? _loadPluginZipJson(PluginRecordDto pluginDataDto){
        if (!TryGetFileBytesFromZip(pluginDataDto.FilePath, "plugin-data.json", out byte[]? rawXmlData)) {
            logger.Error("No 'plugin-data.json' could be found within {path}", pluginDataDto.FilePath);
        }
        string jsonString = Encoding.UTF8.GetString(rawXmlData!);

        throw new NotImplementedException();
    }
    
    private Assembly? _loadPluginZipXml(PluginRecordDto pluginDataDto){
        if (!TryGetFileBytesFromZip(pluginDataDto.FilePath, "plugin-data.xml", out byte[]? rawXmlData)) {
            logger.Error("No 'plugin-data.xml' could be found within {path}", pluginDataDto.FilePath);
        }
        string xmlString = Encoding.UTF8.GetString(rawXmlData!);

        throw new NotImplementedException();
    }
    
    public void LoadPlugin(PluginRecordDto pluginDataDto) {
        // Assembly? assembly = pluginDataDto.PluginType switch {
        //     PluginType.CsProj => _loadPluginCsProj(pluginDataDto),
        //     PluginType.ZipJson => _loadPluginZipJson(pluginDataDto),
        //     PluginType.ZipXml => _loadPluginZipXml(pluginDataDto),
        //     _ => throw new ArgumentOutOfRangeException()
        // };
    }
}