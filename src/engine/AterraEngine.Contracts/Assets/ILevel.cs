// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.ECS;
using AterraEngine.Contracts.ECS.EntityCombinations;
using Raylib_cs;

namespace AterraEngine.Contracts.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ILevel : IEngineAsset{
    public IAssetNodeRoot Assets { get; set; }
    public Color BufferBackground { get; }
    
    public IEntityComponentSystemManager LogicManager {get;set;}
    public IEntityComponentSystemManager RenderManager {get;set;}
    
    IPlayer2DEntity GetPlayer();
    ICamera2D GetCamera();
}