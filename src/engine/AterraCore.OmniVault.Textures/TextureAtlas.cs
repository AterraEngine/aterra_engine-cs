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

            Texture2D texture = Raylib.LoadTextureFromImage(image);
            if (!textureAsset.TrySetTexture(texture)) return false;
            textureAsset.Size = new Vector2(image.Width, image.Height);
            Logger.Debug("Assigned image {path} to asset {Ulid}", textureAsset.ImagePath, textureAsset.InstanceId);
            
            Raylib.UnloadImage(image);
            Logger.Debug("Unloaded image {path}", textureAsset.ImagePath);
            
            Raylib.GenTextureMipmaps(ref texture);
            return true;
        }
        catch (Exception ex) {
            logger.Warning(ex, "Failed without proper exception catching");
            return false;
        }
    }
    public bool TryUnRegisterTexture(AssetId textureAssetId) {
        if(!instanceAtlas.TryGetSingleton(textureAssetId, out ITexture2DAsset? textureAsset)) return false;

        try {
            if (!textureAsset.TryUnSetTexture(out Texture2D texture)) return false;
            Raylib.UnloadTexture(texture);
            Logger.Debug("Unloaded texture {id}", textureAssetId);
            return true;
        }
        catch (Exception ex) {
            logger.Warning(ex, "Failed without proper exception catching");
            return false;
        }
    }
    #endregion
}
