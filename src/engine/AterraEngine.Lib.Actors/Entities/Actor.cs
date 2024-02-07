// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Assets;
using AterraEngine.Contracts.Components;
using AterraEngine.Contracts.ECS.EntityCombinations;
using AterraEngine.Core.Types;
using AterraEngine.Lib.Components;
namespace AterraEngine.Lib.Actors.Entities;

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