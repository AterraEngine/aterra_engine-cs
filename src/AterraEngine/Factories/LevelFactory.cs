// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Actors;
using AterraEngine.Contracts.Atlases;
using AterraEngine.Types;
using AterraEngine.Actors;
using AterraEngine.Contracts.Factories;

namespace AterraEngine.Factories;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LevelFactory(IAssetAtlas assetAtlas, ITexture2DAtlas texture2DAtlas) : ILevelFactory{
    public ILevel CreateLevel(EngineAssetId assetId, string? internalName = null) {
        Level2D level = new Level2D(assetAtlas, texture2DAtlas) {
            Id = assetId,
            InternalName = internalName ?? $"level_{assetId}",
        };

        if (!assetAtlas.TryRegisterAsset(level)) {
            throw new Exception($"Level could not be assigned to the id: {level.InternalName} -> {level.Id}");
        }
        
        return level;
    }
}