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
[TestSubject(typeof(LevelAttribute))]
public class LevelAttributeTest {
    // Test types and interfaces
    [Level("test:asset", CoreTags.Level, ServiceLifetime.Scoped, typeof(ISampleInterface))]
    private class ClassWithLevelAttribute : ISampleInterface;

    [Level<ISampleInterface>("test:generic.asset", CoreTags.Singleton, ServiceLifetime.Singleton)]
    private class ClassWithGenericLevelAttribute : ISampleInterface;

    private interface ISampleInterface;

    private interface IAnotherInterface;

    [Fact]
    public void LevelAttribute_ShouldInitializeCorrectly() {
        const string assetId = "test:sample.asset";
        const CoreTags coreTags = CoreTags.Singleton | CoreTags.Level;
        const ServiceLifetime lifetime = ServiceLifetime.Singleton;
        Type[] interfaceTypes = [typeof(ISampleInterface)];

        var attribute = new LevelAttribute(assetId, coreTags, lifetime, interfaceTypes);

        Assert.Equal(new AssetId(assetId), attribute.AssetId);
        Assert.Equal(coreTags, attribute.CoreTags);
        Assert.Equal(lifetime, attribute.Lifetime);
        Assert.Equal(interfaceTypes, attribute.InterfaceTypes);
    }

    [Fact]
    public void LevelAttribute_Generic_ShouldInitializeCorrectly() {
        const string assetId = "test:sample.generic";
        const CoreTags coreTags = CoreTags.Level;
        const ServiceLifetime lifetime = ServiceLifetime.Scoped;

        var attribute = new LevelAttribute<ISampleInterface>(assetId, coreTags, lifetime);

        Assert.Equal(new AssetId(assetId), attribute.AssetId);
        Assert.Equal(coreTags, attribute.CoreTags);
        Assert.Equal(lifetime, attribute.Lifetime);
        Assert.Equal([typeof(ISampleInterface)], attribute.InterfaceTypes);
    }

    [Fact]
    public void LevelAttribute_Generic_MultipleInterfaces_ShouldInitializeCorrectly() {
        const string assetId = "test:sample.multiple";
        const CoreTags coreTags = CoreTags.Singleton | CoreTags.Level;
        const ServiceLifetime lifetime = ServiceLifetime.Transient;

        // ReSharper disable once RedundantArgumentDefaultValue
        var attribute = new LevelAttribute<ISampleInterface, IAnotherInterface>(assetId, coreTags, lifetime);

        Assert.Equal(new AssetId(assetId), attribute.AssetId);
        Assert.Equal(coreTags, attribute.CoreTags);
        Assert.Equal(lifetime, attribute.Lifetime);
        Assert.Equal([typeof(ISampleInterface), typeof(IAnotherInterface)], attribute.InterfaceTypes);
    }

    [Fact]
    public void ClassWithAttribute_ShouldHaveCorrectAttribute() {
        Type type = typeof(ClassWithLevelAttribute);
        var attribute = (LevelAttribute)type.GetCustomAttribute(typeof(LevelAttribute))!;

        Assert.NotNull(attribute);
        Assert.Equal(new AssetId("test:asset"), attribute.AssetId);
        Assert.True(attribute.CoreTags.HasFlag(CoreTags.Level));
        Assert.Equal(ServiceLifetime.Scoped, attribute.Lifetime);
        Assert.Equal([typeof(ISampleInterface)], attribute.InterfaceTypes);
    }

    [Fact]
    public void ClassWithGenericAttribute_ShouldHaveCorrectAttribute() {
        Type type = typeof(ClassWithGenericLevelAttribute);
        var attribute = (LevelAttribute)type.GetCustomAttribute(typeof(LevelAttribute))!;

        Assert.NotNull(attribute);
        Assert.Equal(new AssetId("test:generic.asset"), attribute.AssetId);
        Assert.True(attribute.CoreTags.HasFlag(CoreTags.Singleton));
        Assert.Equal(ServiceLifetime.Singleton, attribute.Lifetime);
        Assert.Equal([typeof(ISampleInterface)], attribute.InterfaceTypes);
    }
}
