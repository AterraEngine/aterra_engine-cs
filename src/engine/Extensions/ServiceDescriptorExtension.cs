// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
namespace Extensions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ServiceDescriptorExtension {
    public static ServiceDescriptor NewServiceDescriptor<T1, T2>(ServiceLifetime lifetime) where T2 : T1 => new(typeof(T1), typeof(T2), lifetime);
    public static ServiceDescriptor NewServiceDescriptor<T1>(object instance) => new(typeof(T1), instance);
}