// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.PluginConfig;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Boot.Logic.PluginLoading;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace AterraCore.Boot.Logic.PluginLoading;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class FilePathLoadedPluginDto(string filepath) : APluginDto, IFilePathLoadedPluginDto {
    public string FilePath { get; } = filepath;
    private string? _checksum;
    public string CheckSum => _checksum ??= ComputeSha256Hash(FilePath);
    
    public IEnumerable<string> InternalFilePaths { get; set; } = [];
    
    private PluginConfigXml? _configXml;
    public PluginConfigXml ConfigXml {
        get => _configXml!;
        set => _configXml ??= value;
    }
    
    public List<Assembly> Assemblies { get; } = [];
    private IEnumerable<Type>? _types;
    public override IEnumerable<Type> Types => _types ??= Assemblies.SelectMany(assembly => assembly.GetTypes());
    public override PluginId PluginId => _configXml?.NameSpace ?? new PluginId();

    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    private static string ComputeSha256Hash(string filepath) {
        using var mySha256 = SHA256.Create();
        using FileStream stream = File.OpenRead(filepath);
        
        stream.Position = 0;
        
        byte[] hashBytes = mySha256.ComputeHash(stream);
        var builder = new StringBuilder();
        foreach (byte b in hashBytes) {
            builder.Append(b.ToString("x2"));
        }

        return builder.ToString();
    }
}
