// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.PluginConfig;
using AterraCore.Common.Types.FlexiPlug;
using AterraCore.Contracts.Boot.FlexiPlug;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace AterraCore.Boot.Logic.PluginLoading;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PreLoadedPluginDto(string filepath) : IPreLoadedPluginDto {
    public string FilePath { get; } = filepath;
    private string? _checksum;
    public string CheckSum => _checksum ??= ComputeSha256Hash(FilePath);
    
    private PluginValidity _validity = PluginValidity.Untested;
    public PluginValidity Validity {
        get => _validity;
        set => _validity = _validity != PluginValidity.Invalid ? value : _validity;// Once invalid, always invalid
    }
    
    public IEnumerable<string> InternalFilePaths { get; set; } = [];
    
    private PluginConfigXml? _configXml;
    public PluginConfigXml ConfigXml {
        get => _configXml!;
        set => _configXml ??= value;
    }
    
    public List<Assembly> Assemblies { get; } = [];
    private IEnumerable<Type>? _types;
    public IEnumerable<Type> Types => _types ??= Assemblies.SelectMany(assembly => assembly.GetTypes());
    
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

    public void SetInvalid() {
        _validity = PluginValidity.Invalid;
    }
    
    public IEnumerable<(Type Type, T Attribute)> GetOfAttribute<T>() where T : Attribute {
        return Types
            .SelectMany(type => type
                .GetCustomAttributes<T>(false)// this way we only get the attribute once
                .Select(attribute => (Type: type, Attribute: attribute))
            );
    }
}
