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
    [InlineData("abc:def/ghi.jkl")]
    [InlineData("abc-123:def-456")]
    public void TestAssetId(string input) {
        Assert.Matches(RegexLib.AssetId, input);
    }

    [Theory]
    [InlineData("abc:def-")]
    [InlineData(":def")]
    public void TestAssetIdFail(string input) {
        Assert.DoesNotMatch(RegexLib.AssetId, input);
    }

    [Theory]
    [InlineData("asset-123")]
    [InlineData("asset_123")]
    public void TestAssetNamePartial(string input) {
        Assert.Matches(RegexLib.AssetPartial, input);
    }

    [Theory]
    [InlineData("asset-123-")]
    [InlineData("-asset123")]
    [InlineData("asset_123-")]
    public void TestAssetNamePartialFail(string input) {
        Assert.DoesNotMatch(RegexLib.AssetPartial, input);
    }
}
