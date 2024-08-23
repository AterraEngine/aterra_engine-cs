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
public class InjectAsAttribute(string ulid) : Attribute { 
    public Ulid Ulid { get; } = Ulid.Parse(ulid, CultureInfo.InvariantCulture);
}
