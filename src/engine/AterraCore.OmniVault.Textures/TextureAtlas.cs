// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Nexities.Data.Assets;
using AterraCore.Contracts.OmniVault;
using JetBrains.Annotations;
using Raylib_cs;
using Serilog;

namespace AterraCore.OmniVault.Textures;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class TextureAtlas(ILogger logger, IAssetInstanceAtlas instanceAtlas) : ITextureAtlas {
    private ILogger Logger { get; } = logger.ForContext<TextureAtlas>();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryLoadAndRegisterTexture<T>(AssetId textureAssetId, string imagePath, out T? asset, Guid? predefinedGuid = null)
        where T : class, ITexture2DAsset
    {
        asset = default;
        if (!Path.Exists(imagePath)) return false;
        if (!instanceAtlas.TryGetOrCreate(typeof(T), predefinedGuid, out asset)) return false;

        try {
            Image image = Raylib.LoadImage(imagePath);
            Logger.Debug("Loaded image {path}", imagePath);
        
            asset.Texture2D = Raylib.LoadTextureFromImage(image);
            Logger.Debug("Assigned image {path} to asset {guid}", imagePath, asset.Guid);
            
            Raylib.UnloadImage(image);
            Logger.Debug("Unloaded image {path}", imagePath);
            return true;
        }
        catch (Exception ex) {
            logger.Warning(ex, "Failed without proper exception catching");
            asset = default;
            return false;
        }
    }
}
