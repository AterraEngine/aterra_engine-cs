// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Common.Attributes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
[UsedImplicitly]
public class ResolveAsSpecificAttribute(Ulid ulid) : Attribute {
    public Ulid Ulid { get; } = ulid;

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public ResolveAsSpecificAttribute(string ulid) : this(Ulid.Parse(ulid, CultureInfo.InvariantCulture)) {}
    public ResolveAsSpecificAttribute() : this(Ulid.NewUlid()) {}
}