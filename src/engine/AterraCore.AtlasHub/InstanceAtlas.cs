// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using AterraCore.Contracts.AtlasHub;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.FlexiPlug.Assets;
using AterraCore.Types;

namespace AterraCore.AtlasHub;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class InstanceAtlas(IAssetAtlas assetAtlas) : IInstanceAtlas {
    private readonly ConcurrentDictionary<Guid, IAsset> _dictionary = new();
    public IReadOnlyDictionary<Guid, IAsset> Dictionary => _dictionary.AsReadOnly();

    // ---------------------------------------------------------------------------------------------------------------------
    // Methods
    // ---------------------------------------------------------------------------------------------------------------------
    public bool TryCreateInstance<T>(AssetId assetId, T assetDto, [NotNullWhen(true)] out IAsset? asset) where T : AssetDto{
        asset = null;
        if (!assetAtlas.Dictionary.TryGetValue(assetId, out Type? type)) {
            return false;
        }

        try {
            asset = (IAsset)Activator.CreateInstance(type!, [assetDto])!;
            return true;
        }
        catch (MissingMethodException) {
            return false;
        }
    }
}