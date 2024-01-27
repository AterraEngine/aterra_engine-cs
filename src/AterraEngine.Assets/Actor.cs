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