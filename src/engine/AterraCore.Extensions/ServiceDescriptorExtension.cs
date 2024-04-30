// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;

namespace AterraCore.Extensions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ServiceDescriptorExtension {
    public static ServiceDescriptor NewServiceDescriptor<T1, T2>(ServiceLifetime lifetime) where T2 : T1 {
        return new ServiceDescriptor(typeof(T1), typeof(T2), lifetime);
    }
    public static ServiceDescriptor NewServiceDescriptor<T1>(object instance){
        return new ServiceDescriptor(typeof(T1), instance);
    }
}