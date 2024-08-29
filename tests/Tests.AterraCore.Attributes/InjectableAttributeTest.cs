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
    // Sample interfaces for testing
    private interface ISampleInterface1;

    private interface ISampleInterface2;

    private interface ISampleInterface3;

    // Sample classes with InjectableAttribute for testing
    [Injectable(ServiceLifetime.Scoped, true, typeof(ISampleInterface1))]
    private class ClassWithInjectableAttribute : ISampleInterface1;

    [Injectable<ISampleInterface1, ISampleInterface2, ISampleInterface3>(ServiceLifetime.Singleton)]
    private class ClassWithGenericInjectableAttributes : ISampleInterface1, ISampleInterface2, ISampleInterface3;

    [Fact]
    public void InjectableAttribute_ShouldInitializeWithTypeInterfaces() {
        // Arrange
        var serviceLifetime = ServiceLifetime.Scoped;
        bool isStatic = true;
        Type[] interfaces = { typeof(ISampleInterface1), typeof(ISampleInterface2) };

        // Act
        var attribute = new InjectableAttribute(serviceLifetime, isStatic, interfaces);

        // Assert
        Assert.Equal(serviceLifetime, attribute.Lifetime);
        Assert.Equal(isStatic, attribute.IsStatic);
        Assert.Equal(interfaces, attribute.Interfaces);
    }

    [Fact]
    public void InjectableAttribute_Generic_ShouldInitializeWithSingleInterface() {
        // Arrange
        var serviceLifetime = ServiceLifetime.Singleton;
        bool isStatic = true;

        // Act
        var attribute = new InjectableAttribute<ISampleInterface1>(serviceLifetime, isStatic);

        // Assert
        Assert.Equal(serviceLifetime, attribute.Lifetime);
        Assert.Equal(isStatic, attribute.IsStatic);
        Assert.Equal(new[] { typeof(ISampleInterface1) }, attribute.Interfaces);
    }

    [Fact]
    public void InjectableAttribute_Generic_ShouldInitializeWithMultipleInterfaces() {
        // Arrange
        var serviceLifetime = ServiceLifetime.Transient;
        bool isStatic = false;

        // Act
        var attribute = new InjectableAttribute<ISampleInterface1, ISampleInterface2, ISampleInterface3>(serviceLifetime, isStatic);

        // Assert
        Assert.Equal(serviceLifetime, attribute.Lifetime);
        Assert.Equal(isStatic, attribute.IsStatic);
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
        Assert.True(attribute.IsStatic);
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
        Assert.False(attribute.IsStatic);
        Assert.Equal([typeof(ISampleInterface1), typeof(ISampleInterface2), typeof(ISampleInterface3)], attribute.Interfaces);
    }
}
