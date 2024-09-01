// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Boot.Logic.PluginLoading.Dto;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;

namespace AterraCore.Boot.Logic.PluginLoading.Dto;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class RawPluginBootDto(string filepath) : IRawPluginBootDto {
    public string FilePath { get; } = filepath;
    private string? _checksum;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryGetChecksum([NotNullWhen(true)] out string? checksum) {
        if (_checksum is not null) {
            checksum = _checksum;
            return true;
        }

        try {
            using var mySha256 = SHA256.Create();
            using FileStream stream = File.OpenRead(FilePath);

            stream.Position = 0;

            byte[] hashBytes = mySha256.ComputeHash(stream);
            var builder = new StringBuilder();
            foreach (byte b in hashBytes) {
                builder.Append(b.ToString("x2"));
            }

            checksum = _checksum ??= builder.ToString();
            return true;
        }
        catch (Exception) {
            checksum = null;
            return false;
        }
    }
}
