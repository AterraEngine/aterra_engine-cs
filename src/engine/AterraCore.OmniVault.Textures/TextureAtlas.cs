// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Data.Assets;
using AterraCore.Contracts.OmniVault;
using AterraCore.Loggers;
using JetBrains.Annotations;
using Raylib_cs;

using Serilog;

namespace AterraCore.OmniVault.Textures;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class TextureAtlas(ILogger logger, IAssetInstanceAtlas instanceAtlas, IAssetAtlas assetAtlas) : ITextureAtlas {
    public ILogger Logger { get; } = logger.ForTextureAtlasContext();


    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryLoadImage(AssetId assetId, string imagePath, out Texture2D? texture2D) {
        texture2D = default;
        if (!Path.Exists(imagePath)) return false;
        
        Image image = Raylib.LoadImage(imagePath);
        texture2D = Raylib.LoadTextureFromImage(image);
        Raylib.UnloadImage(image);
        
        return true;
    }

    public Texture2D LoadImage(string imagePath) {
        Image image = Raylib.LoadImage(imagePath);
        Texture2D texture2D = Raylib.LoadTextureFromImage(image);
        // Raylib.UnloadImage(image);
        return texture2D;
    }

}
