// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using OldAterraEngine.Contracts.Assets;
using OldAterraEngine.Contracts.Atlases;
using OldAterraEngine.Contracts.DTOs.Assets;
using OldAterraEngine.Contracts.Factories;
using OldAterraEngine.Lib.Actors;
using Raylib_cs;

namespace OldAterraEngine.Core.Factories;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LevelFactory(IAssetAtlas assetAtlas, ITexture2DAtlas texture2DAtlas) : ILevelFactory{
    public ILevel CreateLevel(LevelDto levelDto) {
        var level = new Level2D(levelDto.Id, levelDto.InternalName) {
            BufferBackground = levelDto.BufferBackground ?? Color.Pink
        };

        if (!assetAtlas.TryRegisterAsset(level)) {
            throw new Exception($"Level could not be assigned to the id: {level.InternalName} -> {level.Id}");
        }
        
        return level;
    }
}