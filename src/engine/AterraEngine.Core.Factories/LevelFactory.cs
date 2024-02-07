// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Assets;
using AterraEngine.Contracts.Atlases;
using AterraEngine.Contracts.DTOs.Assets;
using AterraEngine.Contracts.Factories;
using AterraEngine.Lib.Actors;
using Raylib_cs;

namespace AterraEngine.Core.Factories;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LevelFactory(IAssetAtlas assetAtlas, ITexture2DAtlas texture2DAtlas) : ILevelFactory{
    public ILevel CreateLevel(LevelDto levelDto) {
        Level2D level = new Level2D(levelDto.Id, levelDto.InternalName) {
            BufferBackground = levelDto.BufferBackground ?? Color.Pink
        };

        if (!assetAtlas.TryRegisterAsset(level)) {
            throw new Exception($"Level could not be assigned to the id: {level.InternalName} -> {level.Id}");
        }
        
        return level;
    }
}