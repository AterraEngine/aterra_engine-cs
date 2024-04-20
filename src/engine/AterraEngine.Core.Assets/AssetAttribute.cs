// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using AterraCore.Types;

namespace AterraEngine.Core.Assets;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AssetAttribute<TDto>(uint id, AssetType type) : Attribute {
    public uint Id { get; private set; } = id;
    public AssetType Type { get; private set; } = type;
    public Type DtoType { get; private set; } = typeof(TDto);
}