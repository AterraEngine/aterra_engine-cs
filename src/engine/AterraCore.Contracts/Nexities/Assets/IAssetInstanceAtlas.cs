﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using AterraCore.Common.Nexities;

namespace AterraCore.Contracts.Nexities.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IAssetInstanceAtlas {
    public bool TryCreateInstance<T>(AssetId assetId, [NotNullWhen(true)] out T? instance) where T : IAssetInstance;
}