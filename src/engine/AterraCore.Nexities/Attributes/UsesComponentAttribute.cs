// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraCore.Nexities.Attributes;

using AterraCore.Contracts.Nexities.Data.Components;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class UsesComponentAttribute<TComponent>(string? specificId = null) : Attribute where TComponent : IComponent {
    public Type Type { get; } = typeof(TComponent);
    public string? Id { get; } = specificId; // WARN THIS IS A BIG ISSUE! HOW TO DEAL WITH IMPORTING COMPONENTS FROM OTHER PLUGINS?
}