// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine_lib.structs;
using AterraEngine.Interfaces.Component;

namespace AterraEngine.Interfaces.Assets.Lib;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ILevel2D : IAsset {
    public List<EngineAssetId> Assets { get; set; }
    
    public void ResolveAssetIds();
    public void CollideAll();
}