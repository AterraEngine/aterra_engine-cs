// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using JetBrains.Annotations;

namespace AterraEngine.Tests.Core.Types;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(AssetId))]
public class AssetIdTest {
    private static readonly PluginId TestPluginId = new("1234");
    private static readonly PartialAssetId TestPartialAssetId = new("5678abcd");

    [Theory]
    [InlineData("12345678abcd", "1234", "5678abcd")]
    public void AssetId_ConstructorWithString_CorrectlyInitializes(string fullId, string pluginId, string partialId) {
        var assetId = new AssetId(fullId);
        Assert.Equal(pluginId, assetId.PluginId.ToString(), StringComparer.OrdinalIgnoreCase);
        Assert.Equal(partialId, assetId.Id.ToString(), StringComparer.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData("invalid_string")]
    [InlineData("12345678abcz")] // Includes non-hex character 'z'
    [InlineData("")]
    [InlineData(null)]
    public void AssetId_ConstructorWithString_ThrowsForInvalidFormat(string fullId) {
        Assert.Throws<ArgumentException>(() => new AssetId(fullId));
    }

    [Theory]
    [InlineData("12345678abcd", "1234", "5678abcd")]
    public void ToString_ReturnsExpectedFormat(string fullId, string pluginId, string partialId) {
        var assetId = new AssetId(pluginId, partialId);
        Assert.Equal(fullId, assetId.ToString(), StringComparer.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData("1234-5678-abcd", "1234", "5678abcd")]
    public void ToStringReadable_ReturnsExpectedFormat(string fullIdWithDash, string pluginId, string partialId) {
        var assetId = new AssetId(pluginId, partialId);
        Assert.Equal(fullIdWithDash, assetId.ToStringReadable(), StringComparer.OrdinalIgnoreCase);
    }

    [Fact]
    public void AssetId_ConstructorWithPluginIdAndPartialAssetId_CorrectlyInitializes() {
        var assetId = new AssetId(TestPluginId, TestPartialAssetId);
        Assert.Equal(TestPluginId, assetId.PluginId);
        Assert.Equal(TestPartialAssetId, assetId.Id);
    }
    
    [Fact]
    public void Equals_ReturnsExpectedResults() {
        var assetId1 = new AssetId(TestPluginId, TestPartialAssetId);
        var assetId2 = new AssetId(TestPluginId, TestPartialAssetId);
        var assetId3 = new AssetId(new PluginId("abcd"), TestPartialAssetId);

        Assert.True(assetId1.Equals(assetId2));
        Assert.False(assetId1.Equals(assetId3));
    }

    [Fact]
    public void CompareTo_ReturnsExpectedResults() {
        var assetId1 = new AssetId(TestPluginId, TestPartialAssetId);
        var assetId2 = new AssetId(TestPluginId, TestPartialAssetId);
        var assetId3 = new AssetId(new PluginId("abcd"), TestPartialAssetId);

        Assert.Equal(0, assetId1.CompareTo(assetId2));
        Assert.True(assetId1.CompareTo(assetId3) < 0);
        Assert.True(assetId3.CompareTo(assetId1) > 0);
    }

    [Fact]
    public void GetHashCode_IsConsistent() {
        var assetId1 = new AssetId(TestPluginId, TestPartialAssetId);
        var assetId2 = new AssetId(TestPluginId, TestPartialAssetId);

        Assert.Equal(assetId1.GetHashCode(), assetId2.GetHashCode());
    }
}