// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions;

namespace AterraCore.Attributes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// -----------------------------------------------------------------²----------------------------------------------------
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
[UsedImplicitly]
public class AssetTagAttribute(params string[] tags) : Attribute {
    public string[] Tags { get; } = !tags.IsEmpty() ? tags : throw new ArgumentException("Tags cannot be empty.");
}

