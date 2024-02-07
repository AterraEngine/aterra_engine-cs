// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using OldAterraEngine.Contracts.Assets;
using OldAterraEngine.Contracts.Components;
using OldAterraEngine.Contracts.ECS.EntityCombinations;
using OldAterraEngine.Core.Types;
using OldAterraEngine.Lib.Components;
namespace OldAterraEngine.Lib.Actors.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Actor : Asset, IActor, IRender2DEntity {
    private IDraw2DComponent? _drawable; 
    public IDraw2DComponent Drawable => _drawable ??= GetComponent<IDraw2DComponent>(); 
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructor
    // -----------------------------------------------------------------------------------------------------------------
    public Actor(EngineAssetId id, string? internalName) : base(id, internalName) {
        TryAddComponent<ITransform2DComponent, Transform2DComponent>();
        TryAddComponent<IMovement2DComponent, Movement2DComponent>();
        TryAddComponent<IDraw2DComponent, Draw2DComponent>();
        TryAddComponent<IDrawDebug2DComponent, DrawDebug2DComponent>();
    }
}