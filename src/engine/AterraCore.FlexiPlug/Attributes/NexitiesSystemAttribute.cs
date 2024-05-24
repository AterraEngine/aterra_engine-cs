// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
namespace AterraCore.FlexiPlug.Attributes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class)]
[UsedImplicitly]
public class NexitiesSystemAttribute(Type typeInterface, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton) : Attribute {
    public readonly Type Interface = typeInterface;
    public readonly ServiceLifetime Lifetime = serviceLifetime;
}

public class NexitiesSystemAttribute<T>(ServiceLifetime serviceLifetime = ServiceLifetime.Singleton) : NexitiesSystemAttribute(typeof(T), serviceLifetime);