// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Nexities.Data.Assets;

using AterraCore.Nexities.DataParsing.NamedValues.PreMade;
using Contracts.Nexities.Data.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AssetInstance : IAssetInstance {
    [NamedValueGuid("Guid")]
    public Guid Guid { get; internal set; } = new(); // Internal set because we nneed to not have randomly generated when loading from file
}