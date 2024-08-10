// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.Textures;
using JetBrains.Annotations;
using Raylib_cs;
using Serilog;
using System.Numerics;

namespace AterraCore.OmniVault.Textures;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class TextureAtlas(ILogger logger, IAssetInstanceAtlas instanceAtlas) : ITextureAtlas {
    private ILogger Logger { get; } = logger.ForContext<TextureAtlas>();
    private IEnumerable<ITexture2DAsset> TextureAssets => instanceAtlas.OfType<ITexture2DAsset>();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region Registering Texture to GPU
    public bool TryRegisterTexture(AssetId textureAssetId) {
        if (!instanceAtlas.TryGetOrCreateSingleton(textureAssetId, out ITexture2DAsset? textureAsset)) return false;
        if (!Path.Exists(textureAsset.ImagePath)) return false;

        try {
            Image image = Raylib.LoadImage(textureAsset.ImagePath);
            Logger.Debug("Loaded image {path}", textureAsset.ImagePath);
        
            textureAsset.Texture = Raylib.LoadTextureFromImage(image);
            textureAsset.Size = new Vector2(image.Width, image.Height);
            Logger.Debug("Assigned image {path} to asset {Ulid}", textureAsset.ImagePath, textureAsset.Ulid);
            
            Raylib.UnloadImage(image);
            Logger.Debug("Unloaded image {path}", textureAsset.ImagePath);
            return true;
        }
        catch (Exception ex) {
            logger.Warning(ex, "Failed without proper exception catching");
            return false;
        }
    }
    #endregion
}
