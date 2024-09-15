// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.AterraCore.Common.Data;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(RegexLibTest))]
public class RegexLibTest {
    [Theory]
    [InlineData("1.0.0")]
    [InlineData("1.0.0-alpha")]
    public void TestSemanticVersion(string input) {
        Assert.Matches(RegexLib.SemanticVersion, input);
    }

    [Theory]
    [InlineData("1.0")]
    [InlineData("version-1.0.0")]
    public void TestSemanticVersionFail(string input) {
        Assert.DoesNotMatch(RegexLib.SemanticVersion, input);
    }

    [Theory]
    [InlineData("123:456")]
    [InlineData("abc:def/ghi_jkl")]
    [InlineData("abc-123:def-456")]
    public void TestAssetId(string input) {
        Assert.Matches(RegexLib.AssetId, input);
    }

    [Theory]
    [InlineData("abc:def-")]
    [InlineData(":def")]
    [InlineData("abc:def/ghi.jkl")]
    public void TestAssetIdFail(string input) {
        Assert.DoesNotMatch(RegexLib.AssetId, input);
    }

    [Theory]
    [InlineData("asset-123")]
    [InlineData("asset_123")]
    public void TestPluginId(string input) {
        Assert.Matches(RegexLib.PluginId, input);
    }

    [Theory]
    [InlineData("asset-123-")]
    [InlineData("-asset123")]
    [InlineData("asset_123-")]
    public void TestPluginIdFail(string input) {
        Assert.DoesNotMatch(RegexLib.PluginId, input);
    }

    [Theory]
    [InlineData("asset-123")]
    [InlineData("asset_123/test")]
    public void TestNamespaces(string input) {
        Assert.Matches(RegexLib.Namespaces, input);
    }

    [Theory]
    [InlineData("asset-123-")]
    [InlineData("-asset123")]
    [InlineData("asset_123/test-")]
    [InlineData("asset_123/test_")]
    [InlineData("asset_123/test/-")]
    [InlineData("asset_123/test/_")]
    [InlineData("asset_123/test/")]
    [InlineData("asset_123/")]
    public void TestNamespacesFail(string input) {
        Assert.DoesNotMatch(RegexLib.Namespaces, input);
    }
}
