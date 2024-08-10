// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.Assets;
using AterraCore.Contracts.OmniVault.Assets.Attributes;
using JetBrains.Annotations;
using System.Globalization;

namespace AterraCore.OmniVault.Assets.Attributes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents an attribute used to inject a dependency with a specified Ulid.
/// When used in a constructor of an <see cref="IAssetInstance"/>, it will always use an instance of the parameter
/// type which has the same Ulid as the required one.
/// </summary>
/// <param name="ulid">The Ulid, stored in the <see cref="IAssetInstanceAtlas"/>, used to inject in this reference.</param>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
[UsedImplicitly]
public class InjectAsAttribute(string ulid) : IInjectAsAttribute {
    /// <summary>
    /// Represents an attribute used to inject a dependency with a specified Ulid.
    /// </summary>
    public override Ulid Ulid { get; } = Ulid.Parse(ulid, CultureInfo.InvariantCulture);
}
