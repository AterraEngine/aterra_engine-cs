// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;
using AterraEngine.Contracts.Assets;
using AterraEngine.Contracts.Components;

namespace AterraEngine.Contracts.ECS;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IEntity : IEngineAsset {
    public IReadOnlyDictionary<Type, IComponent> Components { get; }

    public bool TryGetComponent<T>([NotNullWhen(true)] out T? component) where T: IComponent;
    
    internal T GetComponent<T>() where T: IComponent; // this should only be used by the Entity Managers
    public bool CheckForComponent(Type t);

    public bool TryAddComponent<T, T2>() where T : IComponent where T2 : T, IComponent, new();
    public bool TryRemoveComponent<T>() where T : IComponent; // Maybe make an editor only version for this?

}