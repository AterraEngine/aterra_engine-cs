// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Contracts.ConfigMancer;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IParsedConfigs {
    int Count { get; }

    bool TryGetConfig<T>([NotNullWhen(true)] out T? value) where T : IConfigMancerElement;
    IReadOnlyDictionary<Type, IConfigMancerElement> AsReadOnlyDictionary();
}
