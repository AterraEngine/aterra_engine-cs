// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using OldAterraEngine.Contracts.Assets;
using OldAterraEngine.Contracts.Components;
using OldAterraEngine.Core.ECS;
using OldAterraEngine.Core.Types;

namespace OldAterraEngine.Lib.Actors.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Asset(EngineAssetId id, string? internalName=null) : Entity(id, internalName) , IAsset {
    private ITransform2DComponent? _transform;
    public ITransform2DComponent Transform => _transform ??= GetComponent<ITransform2DComponent>();

    private IMovement2DComponent? _movement; 
    public IMovement2DComponent Movement => _movement ??= GetComponent<IMovement2DComponent>(); 

}