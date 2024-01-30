// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Components;

namespace AterraEngine.Contracts.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IActor : IAsset{
    public ITransform2DComponent Transform {get;}
    public IMovement2DComponent Movement {get;}
    public IDraw2DComponent Drawable {get;}
}