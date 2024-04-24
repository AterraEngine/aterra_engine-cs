// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace AterraCore.Contracts.FlexiPlug;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IPluginZipImporter<T> {
    bool TryGetPluginConfig([NotNullWhen(true)] out T? pluginConfig);
    bool TryGetPluginAssembly(string filePath, [NotNullWhen(true)] out Assembly? assembly);
    List<string> GetFileNamesInZip();
}