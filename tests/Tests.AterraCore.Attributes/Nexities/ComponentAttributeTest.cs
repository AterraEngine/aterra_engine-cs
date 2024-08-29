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
[TestSubject(typeof(ComponentAttribute))]
public class ComponentAttributeTest {
    // Test types and interfaces
    [Component("test:asset", CoreTags.Component, ServiceLifetime.Scoped, typeof(ISampleInterface))] 
    private class ClassWithComponentAttribute : ISampleInterface;

    [Component<ISampleInterface>("test:generic.asset", CoreTags.Singleton, ServiceLifetime.Singleton)]
    private class ClassWithGenericComponentAttribute : ISampleInterface;

    private interface ISampleInterface;

    private interface IAnotherInterface;

    [Fact]
    public void ComponentAttribute_ShouldInitializeCorrectly() {
        const string assetId = "test:sample.asset";
        const CoreTags coreTags = CoreTags.Singleton | CoreTags.Component;
        const ServiceLifetime lifetime = ServiceLifetime.Singleton;
        Type[] interfaceTypes = [typeof(ISampleInterface)];

        var attribute = new ComponentAttribute(assetId, coreTags, lifetime, interfaceTypes);

        Assert.Equal(new AssetId(assetId), attribute.AssetId);
        Assert.Equal(coreTags, attribute.CoreTags);
        Assert.Equal(lifetime, attribute.Lifetime);
        Assert.Equal(interfaceTypes, attribute.InterfaceTypes);
    }

    [Fact]
    public void ComponentAttribute_Generic_ShouldInitializeCorrectly() {
        const string assetId = "test:sample.generic";
        const CoreTags coreTags = CoreTags.Component;
        const ServiceLifetime lifetime = ServiceLifetime.Scoped;

        var attribute = new ComponentAttribute<ISampleInterface>(assetId, coreTags, lifetime);

        Assert.Equal(new AssetId(assetId), attribute.AssetId);
        Assert.Equal(coreTags, attribute.CoreTags);
        Assert.Equal(lifetime, attribute.Lifetime);
        Assert.Equal([typeof(ISampleInterface)], attribute.InterfaceTypes);
    }

    [Fact]
    public void ComponentAttribute_Generic_MultipleInterfaces_ShouldInitializeCorrectly() {
        const string assetId = "test:sample.multiple";
        const CoreTags coreTags = CoreTags.Singleton | CoreTags.Component;
        const ServiceLifetime lifetime = ServiceLifetime.Transient;

        // ReSharper disable once RedundantArgumentDefaultValue
        var attribute = new ComponentAttribute<ISampleInterface, IAnotherInterface>(assetId, coreTags, lifetime);

        Assert.Equal(new AssetId(assetId), attribute.AssetId);
        Assert.Equal(coreTags, attribute.CoreTags);
        Assert.Equal(lifetime, attribute.Lifetime);
        Assert.Equal([typeof(ISampleInterface), typeof(IAnotherInterface)], attribute.InterfaceTypes);
    }

    [Fact]
    public void ClassWithAttribute_ShouldHaveCorrectAttribute() {
        Type type = typeof(ClassWithComponentAttribute);
        var attribute = (ComponentAttribute)type.GetCustomAttribute(typeof(ComponentAttribute))!;

        Assert.NotNull(attribute);
        Assert.Equal(new AssetId("test:asset"), attribute.AssetId);
        Assert.True(attribute.CoreTags.HasFlag(CoreTags.Component));
        Assert.Equal(ServiceLifetime.Scoped, attribute.Lifetime);
        Assert.Equal([typeof(ISampleInterface)], attribute.InterfaceTypes);
    }

    [Fact]
    public void ClassWithGenericAttribute_ShouldHaveCorrectAttribute() {
        Type type = typeof(ClassWithGenericComponentAttribute);
        var attribute = (ComponentAttribute)type.GetCustomAttribute(typeof(ComponentAttribute))!;

        Assert.NotNull(attribute);
        Assert.Equal(new AssetId("test:generic.asset"), attribute.AssetId);
        Assert.True(attribute.CoreTags.HasFlag(CoreTags.Singleton));
        Assert.Equal(ServiceLifetime.Singleton, attribute.Lifetime);
        Assert.Equal([typeof(ISampleInterface)], attribute.InterfaceTypes);
    }
}
