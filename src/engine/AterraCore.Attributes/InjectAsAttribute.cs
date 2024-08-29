// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using JetBrains.Annotations;
using System.Globalization;

namespace AterraCore.Attributes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
[UsedImplicitly]
public class InjectAsAttribute(string? ulid = null) : Attribute {
    public Ulid Ulid { get; } = ulid is not null
        ? Ulid.Parse(ulid, CultureInfo.InvariantCulture)
        : Ulid.NewUlid(); // Yes this intended behaviour.
}
