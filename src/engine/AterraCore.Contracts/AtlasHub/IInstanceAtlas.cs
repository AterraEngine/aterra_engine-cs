// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using AterraCore.Contracts.FlexiPlug;
using AterraCore.Types;

namespace AterraCore.Contracts.AtlasHub;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IInstanceAtlas {
    public IReadOnlyDictionary<Guid, IAsset> Dictionary { get; }

    public bool TryCreateInstance(AssetId assetId, [NotNullWhen(true)] out IAsset? asset);
    public bool TryCreateInstance<T>(AssetId assetId, [NotNullWhen(true)] out T? asset) where T : IAsset;

}