// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Data;
using AterraCore.Common.Types.Nexities;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.AterraCore.Attributes.Nexities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(SystemAttribute))]
public class SystemAttributeTest {
    // Test types and interfaces
    [System("test:asset", CoreTags.System, ServiceLifetime.Scoped, typeof(ISampleInterface))]
    private class ClassWithSystemAttribute : ISampleInterface;

    [System<ISampleInterface>("test:generic/asset", CoreTags.Singleton, ServiceLifetime.Singleton)]
    private class ClassWithGenericSystemAttribute : ISampleInterface;

    private interface ISampleInterface;

    private interface IAnotherInterface;

    [Fact]
    public void SystemAttribute_ShouldInitializeCorrectly() {
        const string assetId = "test:sample/asset";
        const CoreTags coreTags = CoreTags.Singleton | CoreTags.System;
        const ServiceLifetime lifetime = ServiceLifetime.Singleton;
        Type[] interfaceTypes = [typeof(ISampleInterface)];

        var attribute = new SystemAttribute(assetId, coreTags, lifetime, interfaceTypes);

        Assert.Equal(new AssetId(assetId), attribute.AssetId);
        Assert.Equal(coreTags, attribute.CoreTags);
        Assert.Equal(lifetime, attribute.Lifetime);
        Assert.Equal(interfaceTypes, attribute.InterfaceTypes);
    }

    [Fact]
    public void SystemAttribute_Generic_ShouldInitializeCorrectly() {
        const string assetId = "test:sample/generic";
        const CoreTags coreTags = CoreTags.System;
        const ServiceLifetime lifetime = ServiceLifetime.Scoped;

        var attribute = new SystemAttribute<ISampleInterface>(assetId, coreTags, lifetime);

        Assert.Equal(new AssetId(assetId), attribute.AssetId);
        Assert.Equal(coreTags, attribute.CoreTags);
        Assert.Equal(lifetime, attribute.Lifetime);
        Assert.Equal([typeof(ISampleInterface)], attribute.InterfaceTypes);
    }

    [Fact]
    public void SystemAttribute_Generic_MultipleInterfaces_ShouldInitializeCorrectly() {
        const string assetId = "test:sample/multiple";
        const CoreTags coreTags = CoreTags.Singleton | CoreTags.System;
        const ServiceLifetime lifetime = ServiceLifetime.Transient;

        // ReSharper disable once RedundantArgumentDefaultValue
        var attribute = new SystemAttribute<ISampleInterface, IAnotherInterface>(assetId, coreTags, lifetime);

        Assert.Equal(new AssetId(assetId), attribute.AssetId);
        Assert.Equal(coreTags, attribute.CoreTags);
        Assert.Equal(lifetime, attribute.Lifetime);
        Assert.Equal([typeof(ISampleInterface), typeof(IAnotherInterface)], attribute.InterfaceTypes);
    }

    [Fact]
    public void ClassWithAttribute_ShouldHaveCorrectAttribute() {
        Type type = typeof(ClassWithSystemAttribute);
        var attribute = (SystemAttribute)type.GetCustomAttribute(typeof(SystemAttribute))!;

        Assert.NotNull(attribute);
        Assert.Equal(new AssetId("test:asset"), attribute.AssetId);
        Assert.True(attribute.CoreTags.HasFlag(CoreTags.System));
        Assert.Equal(ServiceLifetime.Scoped, attribute.Lifetime);
        Assert.Equal([typeof(ISampleInterface)], attribute.InterfaceTypes);
    }

    [Fact]
    public void ClassWithGenericAttribute_ShouldHaveCorrectAttribute() {
        Type type = typeof(ClassWithGenericSystemAttribute);
        var attribute = (SystemAttribute)type.GetCustomAttribute(typeof(SystemAttribute))!;

        Assert.NotNull(attribute);
        Assert.Equal(new AssetId("test:generic/asset"), attribute.AssetId);
        Assert.True(attribute.CoreTags.HasFlag(CoreTags.Singleton));
        Assert.Equal(ServiceLifetime.Singleton, attribute.Lifetime);
        Assert.Equal([typeof(ISampleInterface)], attribute.InterfaceTypes);
    }
}
