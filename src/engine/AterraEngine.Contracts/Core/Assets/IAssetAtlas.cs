﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;
using AterraEngine.Core.Types;

namespace AterraEngine.Contracts.Core.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IAssetAtlas {
    public bool TryGetAsset(AssetId assetId, [NotNullWhen(true)] out IAsset? asset);
    public bool TryGetAsset<T>(AssetId assetId, [NotNullWhen(true)] out T? asset) where T : class;

}