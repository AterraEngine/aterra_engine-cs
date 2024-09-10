// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;

namespace Tests.AterraCore.Attributes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(OverridesAssetIdAttribute))]
public class OverridesAssetIdAttributeTests {

    [Fact]
    public void OverridesAssetIdAttribute_ShouldInitializeCorrectly() {
        const string assetId = "test:overridden-asset-id";

        var attribute = new OverridesAssetIdAttribute(assetId);

        Assert.Equal(new AssetId(assetId), attribute.AssetId);
    }

    [Fact]
    public void ClassWithOverridesAssetIdAttribute_ShouldHaveAttributeWithCorrectAssetId() {
        Type type = typeof(ClassWithOverridesAssetId);
        var attribute = (OverridesAssetIdAttribute?)type.GetCustomAttribute(typeof(OverridesAssetIdAttribute));

        Assert.NotNull(attribute);
        Assert.Equal(new AssetId("test:overridden-asset-id"), attribute.AssetId);
    }

    [Fact]
    public void ClassWithMultipleOverridesAssetIdAttributes_ShouldHaveAllAttributes() {
        Type type = typeof(ClassWithMultipleOverridesAssetId);
        OverridesAssetIdAttribute[] attributes = type.GetCustomAttributes<OverridesAssetIdAttribute>().ToArray();

        Assert.NotNull(attributes);
        Assert.Equal(2, attributes.Length);
        Assert.Equal(new AssetId("test:first-override-asset-id"), attributes[0].AssetId);
        Assert.Equal(new AssetId("test:second-override-asset-id"), attributes[1].AssetId);
    }

    // Sample classes for testing
    [OverridesAssetId("test:overridden-asset-id")]
    private class ClassWithOverridesAssetId;

    [OverridesAssetId("test:first-override-asset-id")]
    [OverridesAssetId("test:second-override-asset-id")]
    private class ClassWithMultipleOverridesAssetId;
}
