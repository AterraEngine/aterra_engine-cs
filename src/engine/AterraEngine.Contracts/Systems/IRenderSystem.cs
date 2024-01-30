// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Assets;
using AterraEngine.Contracts.Components;

namespace AterraEngine.Contracts.Systems;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IRenderSystem : ISystem {

    public void LoadTextures(IAsset asset);
    public void UnloadTextures(IAsset asset);
    public void Process(IAsset asset, float deltaTime, ICamera2DComponent camera2DComponent);
}