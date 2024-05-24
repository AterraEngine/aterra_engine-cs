// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Xml.Elements;
namespace AterraCore.Contracts.FlexiPlug;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IPluginZipImporter<T> {
    string CheckSum { get; }
    bool TryGetPluginConfig([NotNullWhen(true)] out T? pluginConfig);
    bool TryGetDllAssembly(IFileDto binDto, [NotNullWhen(true)] out Assembly? assembly);
    List<string> GetFileNamesInZip();
}