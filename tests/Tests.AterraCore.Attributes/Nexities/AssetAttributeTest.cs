// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Attributes.Nexities;
using AterraCore.Common.Data;
using AterraCore.Common.Types.Nexities;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.AterraCore.Attributes.Nexities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(AssetAttribute))]
public class AssetAttributeTest {

    [Fact]
    public void AssetAttribute_ShouldInitializeCorrectly() {
        const string assetId = "test:sample/asset";
        const CoreTags coreTags = CoreTags.Singleton;
        const ServiceLifetime lifetime = ServiceLifetime.Singleton;
        Type[] interfaceTypes = [typeof(ISampleInterface)];

        var attribute = new AssetAttribute(assetId, coreTags, lifetime, interfaceTypes);

        Assert.Equal(new AssetId(assetId), attribute.AssetId);
        Assert.Equal(coreTags, attribute.CoreTags);
        Assert.Equal(lifetime, attribute.Lifetime);
        Assert.Equal(interfaceTypes, attribute.InterfaceTypes);
    }

    [Fact]
    public void AssetAttribute_Generic_ShouldInitializeCorrectly() {
        const string assetId = "test:sample/generic";
        const CoreTags coreTags = CoreTags.Asset;
        const ServiceLifetime lifetime = ServiceLifetime.Scoped;

        var attribute = new AssetAttribute<ISampleInterface>(assetId, coreTags, lifetime);

        Assert.Equal(new AssetId(assetId), attribute.AssetId);
        Assert.Equal(coreTags, attribute.CoreTags);
        Assert.Equal(lifetime, attribute.Lifetime);
        Assert.Equal([typeof(ISampleInterface)], attribute.InterfaceTypes);
    }

    [Fact]
    public void AssetAttribute_Generic_MultipleInterfaces_ShouldInitializeCorrectly() {
        const string assetId = "test:sample/multiple";
        const CoreTags coreTags = CoreTags.Singleton;
        const ServiceLifetime lifetime = ServiceLifetime.Transient;

        // ReSharper disable once RedundantArgumentDefaultValue
        var attribute = new AssetAttribute<ISampleInterface, IAnotherInterface>(assetId, coreTags, lifetime);

        Assert.Equal(new AssetId(assetId), attribute.AssetId);
        Assert.Equal(coreTags, attribute.CoreTags);
        Assert.Equal(lifetime, attribute.Lifetime);
        Assert.Equal([typeof(ISampleInterface), typeof(IAnotherInterface)], attribute.InterfaceTypes);
    }

    [Fact]
    public void ClassWithAttribute_ShouldHaveCorrectAttribute() {
        Type type = typeof(ClassWithAssetAttribute);
        var attribute = (AssetAttribute)type.GetCustomAttribute(typeof(AssetAttribute))!;

        Assert.NotNull(attribute);
        Assert.Equal(new AssetId("test:asset"), attribute.AssetId);
        Assert.Equal(CoreTags.Asset, attribute.CoreTags);
        Assert.Equal(ServiceLifetime.Scoped, attribute.Lifetime);
        Assert.Equal([typeof(ISampleInterface)], attribute.InterfaceTypes);
    }

    [Fact]
    public void ClassWithGenericAttribute_ShouldHaveCorrectAttribute() {
        Type type = typeof(ClassWithGenericAssetAttribute);
        var attribute = (AssetAttribute)type.GetCustomAttribute(typeof(AssetAttribute))!;

        Assert.NotNull(attribute);
        Assert.Equal(new AssetId("test:generic/asset"), attribute.AssetId);
        Assert.Equal(CoreTags.Singleton, attribute.CoreTags);
        Assert.Equal(ServiceLifetime.Singleton, attribute.Lifetime);
        Assert.Equal([typeof(ISampleInterface)], attribute.InterfaceTypes);
    }

    // Test types and interfaces
    [Asset("test:asset", CoreTags.Asset, ServiceLifetime.Scoped, typeof(ISampleInterface))]
    private class ClassWithAssetAttribute : ISampleInterface;

    [Asset<ISampleInterface>("test:generic/asset", CoreTags.Singleton, ServiceLifetime.Singleton)]
    private class ClassWithGenericAssetAttribute : ISampleInterface;

    private interface ISampleInterface;

    private interface IAnotherInterface;
}
