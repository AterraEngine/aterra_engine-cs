// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.OldContracts.Assets;
using AterraEngine.OldContracts.Components;

namespace AterraEngine.OldContracts.Systems;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IRenderSystem : ISystem {

    public void LoadTextures(IAsset asset);
    public void UnloadTextures(IAsset asset);
    public void Process(IAsset asset, float deltaTime, ICamera2DComponent camera2DComponent);
}