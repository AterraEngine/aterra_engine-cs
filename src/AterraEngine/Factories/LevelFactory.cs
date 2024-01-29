// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.OldContracts.Assets;
using AterraEngine.OldContracts.Atlases;
using AterraEngine.Types;
using AterraEngine.Assets;
using AterraEngine.OldContracts.Factories;
using AterraEngine.DTO.Assets;
using Raylib_cs;

namespace AterraEngine.Factories;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LevelFactory(IAssetAtlas assetAtlas, ITexture2DAtlas texture2DAtlas) : ILevelFactory{
    public ILevel CreateLevel(LevelDto levelDto) {
        Level2D level = new Level2D(levelDto.Id, levelDto.InternalName);
        level.BufferBackground = levelDto.BufferBackground ?? Color.Pink;
        
        if (!assetAtlas.TryRegisterAsset(level)) {
            throw new Exception($"Level could not be assigned to the id: {level.InternalName} -> {level.Id}");
        }
        
        return level;
    }
}