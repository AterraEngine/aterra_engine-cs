// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;
using AterraEngine.OldContracts.Components;

namespace AterraEngine.OldContracts.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IAsset : IEngineAsset{
    public IReadOnlyDictionary<Type, IComponent> Components { get; }

    public bool TryGetComponent<T>([NotNullWhen(true)] out T? component) where T: IComponent;

    public bool TryAddComponent<T, T2>() where T : IComponent where T2 : T, IComponent, new();
    
    public bool TryRemoveComponent<T>() where T : IComponent; // Maybe make an editor only version for this?
}