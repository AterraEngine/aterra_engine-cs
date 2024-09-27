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
[TestSubject(typeof(EntityAttribute))]
public class EntityAttributeTest {

    [Fact]
    public void EntityAttribute_ShouldInitializeCorrectly() {
        const string assetId = "test:sample/asset";
        const CoreTags coreTags = CoreTags.Singleton | CoreTags.Entity;
        const ServiceLifetime lifetime = ServiceLifetime.Singleton;
        Type[] interfaceTypes = [typeof(ISampleInterface)];

        var attribute = new EntityAttribute(assetId, coreTags, lifetime, interfaceTypes);

        Assert.Equal(new AssetId(assetId), attribute.AssetId);
        Assert.Equal(coreTags, attribute.CoreTags);
        Assert.Equal(lifetime, attribute.Lifetime);
        Assert.Equal(interfaceTypes, attribute.InterfaceTypes);
    }

    [Fact]
    public void EntityAttribute_Generic_ShouldInitializeCorrectly() {
        const string assetId = "test:sample/generic";
        const CoreTags coreTags = CoreTags.Entity;
        const ServiceLifetime lifetime = ServiceLifetime.Scoped;

        var attribute = new EntityAttribute<ISampleInterface>(assetId, coreTags, lifetime);

        Assert.Equal(new AssetId(assetId), attribute.AssetId);
        Assert.Equal(coreTags, attribute.CoreTags);
        Assert.Equal(lifetime, attribute.Lifetime);
        Assert.Equal([typeof(ISampleInterface)], attribute.InterfaceTypes);
    }

    [Fact]
    public void EntityAttribute_Generic_MultipleInterfaces_ShouldInitializeCorrectly() {
        const string assetId = "test:sample/multiple";
        const CoreTags coreTags = CoreTags.Singleton | CoreTags.Entity;
        const ServiceLifetime lifetime = ServiceLifetime.Transient;

        // ReSharper disable once RedundantArgumentDefaultValue
        var attribute = new EntityAttribute<ISampleInterface, IAnotherInterface>(assetId, coreTags, lifetime);

        Assert.Equal(new AssetId(assetId), attribute.AssetId);
        Assert.Equal(coreTags, attribute.CoreTags);
        Assert.Equal(lifetime, attribute.Lifetime);
        Assert.Equal([typeof(ISampleInterface), typeof(IAnotherInterface)], attribute.InterfaceTypes);
    }

    [Fact]
    public void ClassWithAttribute_ShouldHaveCorrectAttribute() {
        Type type = typeof(ClassWithEntityAttribute);
        var attribute = (EntityAttribute)type.GetCustomAttribute(typeof(EntityAttribute))!;

        Assert.NotNull(attribute);
        Assert.Equal(new AssetId("test:asset"), attribute.AssetId);
        Assert.True(attribute.CoreTags.HasFlag(CoreTags.Entity));
        Assert.Equal(ServiceLifetime.Scoped, attribute.Lifetime);
        Assert.Equal([typeof(ISampleInterface)], attribute.InterfaceTypes);
    }

    [Fact]
    public void ClassWithGenericAttribute_ShouldHaveCorrectAttribute() {
        Type type = typeof(ClassWithGenericEntityAttribute);
        var attribute = (EntityAttribute)type.GetCustomAttribute(typeof(EntityAttribute))!;

        Assert.NotNull(attribute);
        Assert.Equal(new AssetId("test:generic/asset"), attribute.AssetId);
        Assert.True(attribute.CoreTags.HasFlag(CoreTags.Singleton));
        Assert.Equal(ServiceLifetime.Singleton, attribute.Lifetime);
        Assert.Equal([typeof(ISampleInterface)], attribute.InterfaceTypes);
    }

    // Test types and interfaces
    [Entity("test:asset", CoreTags.Entity, ServiceLifetime.Scoped, typeof(ISampleInterface))]
    private class ClassWithEntityAttribute : ISampleInterface;

    [Entity<ISampleInterface>("test:generic/asset", CoreTags.Singleton, ServiceLifetime.Singleton)]
    private class ClassWithGenericEntityAttribute : ISampleInterface;

    private interface ISampleInterface;

    private interface IAnotherInterface;
}
