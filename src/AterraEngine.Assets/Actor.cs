// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Components;
using AterraEngine.Contracts.Assets;
using AterraEngine.Contracts.Components;
using AterraEngine.Types;

namespace AterraEngine.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Actor : Asset, IActor {
    private ITransform2DComponent? _transform;
    public ITransform2DComponent Transform => _transform ??= GetComponent<ITransform2DComponent>();

    private IMovement2DComponent? _movement; 
    public IMovement2DComponent Movement => _movement ??= GetComponent<IMovement2DComponent>(); 
    
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