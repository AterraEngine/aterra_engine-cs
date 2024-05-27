﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraCore.Contracts.Nexities.Data.Assets;

using Common.Types.Nexities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface IAssetDto {
    AssetId AssetId { get; }
    Guid[] StartingComponentGuids { get; }
}