// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.AterraCore.Attributes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(AssetTagAttribute))]
public class AssetTagAttributeTests {

    [Fact]
    public void AssetTagAttribute_ShouldInitializeWithTags() {
        string[] tags = { "Tag1", "Tag2", "Tag3" };

        // Act
        var attribute = new AssetTagAttribute(tags);

        Assert.Equal(tags, attribute.Tags);
    }

    [Fact]
    public void AssetTagAttribute_ShouldNotHandleEmptyTags() {
        Assert.Throws<ArgumentException>(() => new AssetTagAttribute());
    }

    [Fact]
    public void ClassWithAssetTagAttribute_ShouldHaveCorrectTags() {
        Type type = typeof(ClassWithAssetTag);
        var attribute = (AssetTagAttribute?)type.GetCustomAttribute(typeof(AssetTagAttribute));

        Assert.NotNull(attribute);
        Assert.Equal(new[] { "Feature", "Beta" }, attribute.Tags);
    }

    [Fact]
    public void ClassWithMultipleAssetTagAttributes_ShouldHaveAllTags() {
        Type type = typeof(ClassWithMultipleAssetTags);
        AssetTagAttribute[] attributes = type.GetCustomAttributes<AssetTagAttribute>().ToArray();

        Assert.NotNull(attributes);
        Assert.Equal(3, attributes.Length);

        // Flatten the tags from all attributes and assert they contain expected tags.
        string[] allTags = attributes.SelectMany(attr => attr.Tags).ToArray();
        string[] expectedTags = ["Feature", "Beta", "Production", "Critical", "NeedsReview"];

        Assert.Equal(expectedTags, allTags);
    }

    // Sample classes with AssetTagAttribute for testing
    [AssetTag("Feature", "Beta")]
    private class ClassWithAssetTag;

    [AssetTag("Feature")]
    [AssetTag("Beta", "Production")]
    [AssetTag("Critical", "NeedsReview")]
    private class ClassWithMultipleAssetTags;
}
