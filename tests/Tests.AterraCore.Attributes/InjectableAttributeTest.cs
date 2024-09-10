// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;

namespace Tests.AterraCore.Attributes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(InjectableAttribute))]
public class InjectableAttributeTests {

    [Fact]
    public void InjectableAttribute_ShouldInitializeWithTypeInterfaces() {
        // Arrange
        const ServiceLifetime serviceLifetime = ServiceLifetime.Scoped;
        Type[] interfaces = [typeof(ISampleInterface1), typeof(ISampleInterface2)];

        // Act
        var attribute = new InjectableAttribute(serviceLifetime, interfaces);

        // Assert
        Assert.Equal(serviceLifetime, attribute.Lifetime);
        Assert.Equal(interfaces, attribute.Interfaces);
    }

    [Fact]
    public void InjectableAttribute_Generic_ShouldInitializeWithSingleInterface() {
        // Arrange
        const ServiceLifetime serviceLifetime = ServiceLifetime.Singleton;

        // Act
        var attribute = new InjectableAttribute<ISampleInterface1>(serviceLifetime);

        // Assert
        Assert.Equal(serviceLifetime, attribute.Lifetime);
        Assert.Equal([typeof(ISampleInterface1)], attribute.Interfaces);
    }

    [Fact]
    public void InjectableAttribute_Generic_ShouldInitializeWithMultipleInterfaces() {
        // Arrange
        const ServiceLifetime serviceLifetime = ServiceLifetime.Transient;

        // Act
        var attribute = new InjectableAttribute<ISampleInterface1, ISampleInterface2, ISampleInterface3>(serviceLifetime);

        // Assert
        Assert.Equal(serviceLifetime, attribute.Lifetime);
        Assert.Equal([typeof(ISampleInterface1), typeof(ISampleInterface2), typeof(ISampleInterface3)], attribute.Interfaces);
    }

    [Fact]
    public void ClassWithInjectableAttribute_ShouldHaveCorrectAttribute() {
        // Arrange & Act
        Type type = typeof(ClassWithInjectableAttribute);
        var attribute = (InjectableAttribute?)type.GetCustomAttribute(typeof(InjectableAttribute));

        // Assert
        Assert.NotNull(attribute);
        Assert.Equal(ServiceLifetime.Scoped, attribute.Lifetime);
        Assert.Equal([typeof(ISampleInterface1)], attribute.Interfaces);
    }

    [Fact]
    public void ClassWithGenericInjectableAttributes_ShouldHaveCorrectAttributes() {
        // Arrange & Act
        Type type = typeof(ClassWithGenericInjectableAttributes);
        var attribute = (InjectableAttribute?)type.GetCustomAttribute(typeof(InjectableAttribute));

        // Assert
        Assert.NotNull(attribute);
        Assert.Equal(ServiceLifetime.Singleton, attribute.Lifetime);
        Assert.Equal([typeof(ISampleInterface1), typeof(ISampleInterface2), typeof(ISampleInterface3)], attribute.Interfaces);
    }

    // Sample interfaces for testing
    private interface ISampleInterface1;

    private interface ISampleInterface2;

    private interface ISampleInterface3;

    // Sample classes with InjectableAttribute for testing
    [Injectable(ServiceLifetime.Scoped, typeof(ISampleInterface1))]
    private class ClassWithInjectableAttribute : ISampleInterface1;

    [Injectable<ISampleInterface1, ISampleInterface2, ISampleInterface3>(ServiceLifetime.Singleton)]
    private class ClassWithGenericInjectableAttributes : ISampleInterface1, ISampleInterface2, ISampleInterface3;
}
