// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.Assets.Attributes;

namespace AterraCore.OmniVault.Assets.Attributes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents an attribute used to inject a dependency with a specified GUID.
/// When used in a constructor of an <see cref="IAssetInstance"/>, it will always use an instance of the parameter
/// type which has the same guid as the required one.
/// </summary>
/// <param name="guid">The GUID, stored in the <see cref="IAssetInstanceAtlas"/>, used to inject in this reference.</param>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
public class InjectAsAttribute(string guid) : IInjectAsAttribute {
    /// <summary>
    /// Represents an attribute used to inject a dependency with a specified GUID.
    /// </summary>
    public override Guid Guid { get; } = Guid.Parse(guid);
}
