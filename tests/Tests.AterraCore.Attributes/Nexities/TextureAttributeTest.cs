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
[TestSubject(typeof(TextureAttribute))]
public class TextureAttributeTest {

    [Fact]
    public void TextureAttribute_ShouldInitializeCorrectly() {
        const string assetId = "test:sample/asset";
        const CoreTags coreTags = CoreTags.Singleton | CoreTags.Texture;
        const ServiceLifetime lifetime = ServiceLifetime.Singleton;
        Type[] interfaceTypes = [typeof(ISampleInterface)];

        var attribute = new TextureAttribute(assetId, coreTags, lifetime, interfaceTypes);

        Assert.Equal(new AssetId(assetId), attribute.AssetId);
        Assert.Equal(coreTags, attribute.CoreTags);
        Assert.Equal(lifetime, attribute.Lifetime);
        Assert.Equal(interfaceTypes, attribute.InterfaceTypes);
    }

    [Fact]
    public void TextureAttribute_Generic_ShouldInitializeCorrectly() {
        const string assetId = "test:sample/generic";
        const CoreTags coreTags = CoreTags.Texture | CoreTags.Singleton;
        const ServiceLifetime lifetime = ServiceLifetime.Scoped;

        var attribute = new TextureAttribute<ISampleInterface>(assetId, coreTags, lifetime);

        Assert.Equal(new AssetId(assetId), attribute.AssetId);
        Assert.Equal(coreTags, attribute.CoreTags);
        Assert.Equal(lifetime, attribute.Lifetime);
        Assert.Equal([typeof(ISampleInterface)], attribute.InterfaceTypes);
    }

    [Fact]
    public void TextureAttribute_Generic_MultipleInterfaces_ShouldInitializeCorrectly() {
        const string assetId = "test:sample/multiple";
        const CoreTags coreTags = CoreTags.Singleton | CoreTags.Texture;
        const ServiceLifetime lifetime = ServiceLifetime.Transient;

        // ReSharper disable once RedundantArgumentDefaultValue
        var attribute = new TextureAttribute<ISampleInterface, IAnotherInterface>(assetId, coreTags, lifetime);

        Assert.Equal(new AssetId(assetId), attribute.AssetId);
        Assert.Equal(coreTags, attribute.CoreTags);
        Assert.Equal(lifetime, attribute.Lifetime);
        Assert.Equal([typeof(ISampleInterface), typeof(IAnotherInterface)], attribute.InterfaceTypes);
    }

    [Fact]
    public void ClassWithAttribute_ShouldHaveCorrectAttribute() {
        Type type = typeof(ClassWithTextureAttribute);
        var attribute = (TextureAttribute)type.GetCustomAttribute(typeof(TextureAttribute))!;

        Assert.NotNull(attribute);
        Assert.Equal(new AssetId("test:asset"), attribute.AssetId);
        Assert.True(attribute.CoreTags.HasFlag(CoreTags.Texture));
        Assert.Equal(ServiceLifetime.Scoped, attribute.Lifetime);
        Assert.Equal([typeof(ISampleInterface)], attribute.InterfaceTypes);
    }

    [Fact]
    public void ClassWithGenericAttribute_ShouldHaveCorrectAttribute() {
        Type type = typeof(ClassWithGenericTextureAttribute);
        var attribute = (TextureAttribute)type.GetCustomAttribute(typeof(TextureAttribute))!;

        Assert.NotNull(attribute);
        Assert.Equal(new AssetId("test:generic/asset"), attribute.AssetId);
        Assert.True(attribute.CoreTags.HasFlag(CoreTags.Singleton));
        Assert.Equal(ServiceLifetime.Singleton, attribute.Lifetime);
        Assert.Equal([typeof(ISampleInterface)], attribute.InterfaceTypes);
    }

    // Test types and interfaces
    [Texture("test:asset", CoreTags.Texture, ServiceLifetime.Scoped, typeof(ISampleInterface))]
    private class ClassWithTextureAttribute : ISampleInterface;

    [Texture<ISampleInterface>("test:generic/asset", CoreTags.Singleton)]
    private class ClassWithGenericTextureAttribute : ISampleInterface;

    private interface ISampleInterface;

    private interface IAnotherInterface;
}
