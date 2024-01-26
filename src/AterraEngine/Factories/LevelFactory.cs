// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Assets;
using AterraEngine.Contracts.Atlases;
using AterraEngine.Types;
using AterraEngine.Assets;
using AterraEngine.Contracts.Factories;
using AterraEngine.DTO.Assets;

namespace AterraEngine.Factories;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LevelFactory(IAssetAtlas assetAtlas, ITexture2DAtlas texture2DAtlas) : ILevelFactory{
    public ILevel CreateLevel(LevelDto levelDto) {
        Level2D level = new Level2D(assetAtlas, texture2DAtlas);
        level.PopulateFromDto(levelDto);

        if (!assetAtlas.TryRegisterAsset(level)) {
            throw new Exception($"Level could not be assigned to the id: {level.InternalName} -> {level.Id}");
        }
        
        return level;
    }
}