// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Reflection;

namespace Tests.AterraCore.Common.Data;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(AssetRegistration))]
public class AssetRegistrationTest {
    // Sample types for testing
    private class SampleAsset : ISampleInterface;

    private class SecondSampleAsset : ISampleInterface;

    private class SingletonAsset : ISampleInterface;

    private class DerivedAsset : SampleAsset, IDerivedInterface;

    private interface ISampleInterface;

    private interface IDerivedInterface : ISampleInterface;

    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly() {
        var assetId = new AssetId("test:sample.asset");
        Type type = typeof(SampleAsset);
        const CoreTags coreTags = CoreTags.Singleton;
        string[] stringTags = ["Tag1", "Tag2"];
        AssetId[] overridableAssetIds = [new("test:base.asset")];

        var registration = new AssetRegistration(
            assetId,
            type
        ) {
            CoreTags = coreTags,
            StringTags = stringTags,
            OverridableAssetIds = overridableAssetIds
        };

        Assert.Equal(assetId, registration.AssetId);
        Assert.Equal(type, registration.Type);
        Assert.Equal(coreTags, registration.CoreTags);
        Assert.Equal(stringTags, registration.StringTags);
        Assert.Equal(overridableAssetIds, registration.OverridableAssetIds);
    }

    [Fact]
    public void Constructor_ShouldProvideFirstConstructor() {
        Type type = typeof(SampleAsset);

        var registration = new AssetRegistration(
            new AssetId("test:sample.asset"),
            type
        );

        ConstructorInfo expectedConstructor = type.GetConstructors().First();
        Assert.Equal(expectedConstructor, registration.Constructor);
    }

    [Fact]
    public void Constructor_InterfacesInit() {
        Type type = typeof(SampleAsset);

        var registration = new AssetRegistration(
            new AssetId("test:sample.asset"),
            type
        ) {
            InterfaceTypes = [typeof(ISampleInterface), typeof(IDerivedInterface)]
        };

        Type[] expectedInterfaces = [typeof(ISampleInterface), typeof(IDerivedInterface)];
        Assert.Subset(new HashSet<Type>(expectedInterfaces), new HashSet<Type>(registration.DerivedInterfaceTypes));

    }

    [Fact]
    public void IsSingleton_ShouldReturnCorrectly() {
        Type type = typeof(SingletonAsset);

        var registrationWithSingleton = new AssetRegistration(
            new AssetId("test:singleton.asset"),
            type
        ) {
            CoreTags = CoreTags.Singleton
        };

        var registrationWithoutSingleton = new AssetRegistration(
            new AssetId("test:non-singleton.asset"),
            type
        ) {
            CoreTags = 0
        };

        Assert.True(registrationWithSingleton.IsSingleton);
        Assert.False(registrationWithoutSingleton.IsSingleton);
    }


    [Fact]
    public void DerivedInterfaceTypes_ShouldContainCorrectInterfaces() {
        Type type = typeof(DerivedAsset);

        var registration = new AssetRegistration(
            new AssetId("test:derived.asset"),
            type
        );

        Type[] expectedInterfaces = [typeof(ISampleInterface), typeof(IDerivedInterface)];
        Assert.Subset(new HashSet<Type>(expectedInterfaces), new HashSet<Type>(registration.DerivedInterfaceTypes));
    }

    [Fact]
    public void InterfaceTypes_ShouldInitializeToEmpty() {
        var registration = new AssetRegistration(
            new AssetId("test:sample.asset"),
            typeof(SampleAsset)
        );

        Assert.Empty(registration.InterfaceTypes);
    }

    [Fact]
    public void AssetId_Property_ShouldReturnCorrectValue() {
        var assetId = new AssetId("test:sample.asset");

        var registration = new AssetRegistration(
            assetId,
            typeof(SampleAsset)
        );

        Assert.Equal(assetId, registration.AssetId);
    }

    [Fact]
    public void Type_Property_ShouldReturnCorrectValue() {
        Type type = typeof(SampleAsset);

        var registration = new AssetRegistration(
            new AssetId("test:sample.asset"),
            type
        );

        Assert.Equal(type, registration.Type);
    }

    [Fact]
    public void Type_Property_Set() {
        Type type = typeof(SampleAsset);
        Type typeNew = typeof(SecondSampleAsset);

        var registration = new AssetRegistration(
            new AssetId("test:sample.asset"),
            type
        );

        registration.Type = typeNew;

        Assert.Equal(typeNew, registration.Type);
    }

    [Fact]
    public void AssetId_Property_Set() {
        var idNew = new AssetId("test:sample.asset.new");
        Type type = typeof(SampleAsset);

        var registration = new AssetRegistration(
            new AssetId("test:sample.asset"),
            type
        );

        registration.AssetId = idNew;

        Assert.Equal(idNew, registration.AssetId);
    }
}
