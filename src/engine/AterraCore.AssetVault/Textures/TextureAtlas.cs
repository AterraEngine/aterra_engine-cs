// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.DI;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.Textures;
using JetBrains.Annotations;
using Raylib_cs;
using Serilog;
using System.Numerics;

namespace AterraCore.AssetVault.Textures;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Singleton<ITextureAtlas>]
public class TextureAtlas(ILogger logger, IAssetInstanceAtlas instanceAtlas, IPluginAtlas pluginAtlas) : ITextureAtlas {
    private ILogger Logger { get; } = logger.ForContext<TextureAtlas>();
    public IEnumerable<ITexture2DAsset> TextureAssets => instanceAtlas.OfType<ITexture2DAsset>();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private bool TryGetImage(AssetId assetId, string filePath, out Image image) {
        image = new Image();
        if (!pluginAtlas.PluginIds.Contains(assetId.PluginId)) {
            if (!Path.Exists(filePath)) return false;

            // Image is not part of a plugin, and can be loaded directly
            image = Raylib.LoadImage(filePath);
            return true;
        }

        if (!pluginAtlas.TryGetFileRawFromPluginZip(assetId.PluginId, filePath, out byte[]? rawImage)) return false;

        logger.Debug($"Loading image {assetId} && {filePath}");
        image = Raylib.LoadImageFromMemory(Path.GetExtension(filePath), rawImage);
        return true;
    }

    #region Registering Texture to GPU
    public bool TryRegisterTexture(AssetId textureAssetId) {
        if (!instanceAtlas.TryGetOrCreateSingleton(textureAssetId, out ITexture2DAsset? textureAsset)) return false;

        try {
            if (!TryGetImage(textureAssetId, textureAsset.ImagePath, out Image image)) return false;

            Logger.Debug("Loaded image {path}", textureAsset.ImagePath);

            Texture2D texture = Raylib.LoadTextureFromImage(image);
            if (!textureAsset.TrySetTexture(texture)) return false;

            textureAsset.Size = new Vector2(image.Width, image.Height);
            Logger.Debug("Assigned image {path} to asset {Ulid}", textureAsset.ImagePath, textureAsset.InstanceId);

            Raylib.UnloadImage(image);
            Logger.Debug("Unloaded image {path}", textureAsset.ImagePath);

            Raylib.GenTextureMipmaps(ref texture);
            Raylib.SetTextureFilter(texture, TextureFilter.Anisotropic4X);
            
            return true;
        }
        catch (Exception ex) {
            logger.Warning(ex, "Failed without proper exception catching for id: {textureAssetId}", textureAssetId);
            return false;
        }
    }
    public bool TryUnRegisterTexture(AssetId textureAssetId) {
        if (!instanceAtlas.TryGetSingleton(textureAssetId, out ITexture2DAsset? textureAsset)) return false;

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
