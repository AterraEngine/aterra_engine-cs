// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace AterraCore.Attributes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class)]
[UsedImplicitly]
public class InjectableAttribute(Type typeInterface, ServiceLifetime serviceLifetime = ServiceLifetime.Transient, bool @static=false) : Attribute {
    public readonly Type Interface = typeInterface;
    public readonly ServiceLifetime Lifetime = serviceLifetime;
    public readonly bool IsStatic = @static;
}

[UsedImplicitly] public class InjectableAttribute<T>(ServiceLifetime serviceLifetime = ServiceLifetime.Transient, bool @static=false) : InjectableAttribute(typeof(T), serviceLifetime, @static);
