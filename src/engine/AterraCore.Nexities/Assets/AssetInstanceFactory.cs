// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using AterraCore.Contracts.AtlasHub;
using AterraCore.Contracts.Nexities;
using AterraCore.Types;

namespace AterraCore.Nexities.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class AssetInstanceFactory(IAssetAtlas assetAtlas, IInstanceAtlas instanceAtlas) {

    public bool TryCreateAssetInstance<T>(AssetId assetId, [NotNullWhen(true)] out T? asset) where T : IAsset {
        asset = default;
        if(!assetAtlas.TryGetAssetRecord(assetId, out AssetRecord? assetRecord)) {
            // TODO LOG ERROR
            return false;
        }

        switch (assetRecord) {
            case { InstanceType: AssetInstanceType.Singleton }:
                // FIND THE STUPID THING IN A DIFFERENT DICTIONARY
                break;
            case { InstanceType: AssetInstanceType.Transient }:
                return instanceAtlas.TryCreateInstance(assetId, CreateConstructor(), out asset);
            default:
                throw new ArgumentOutOfRangeException();
        }

        return false;
    }

    public AssetConstructorDto CreateConstructor() {
        return new AssetConstructorDto(, Array.Empty<Guid>());
    }

}