﻿// ---------------------------------------------------------------------------------------------------------------------
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
public class ServiceAttribute(Type typeInterface, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton) : Attribute {
    public readonly Type Interface = typeInterface;
    public readonly ServiceLifetime Lifetime = serviceLifetime;
}