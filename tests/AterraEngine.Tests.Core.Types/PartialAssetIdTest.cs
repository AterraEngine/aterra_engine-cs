// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using JetBrains.Annotations;
namespace AterraEngine.Tests.Core.Types;

using AterraCore.Common.Types.Nexities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(PartialAssetId))]
public class PartialAssetIdTest {
    [Theory]
    [InlineData(123456u)]
    [InlineData(789012u)]
    public void Ctor_ValueParameter_CorrectSetterGetters(uint input) {
        var output = new PartialAssetId(input);
        Assert.Equal(input, output.Value);
    }

    [Fact]
    public void Ctor_DefaultValueParameter_CorrectSetterGetters() {
        var output = new PartialAssetId();
        Assert.Equal(0u, output.Value);
    }

    [Theory]
    [InlineData("INVALID_FORMAT", false)]
    [InlineData("7593F98A", true)]
    public void TryParse(string input, bool result) {
        Assert.Equal(result, PartialAssetId.TryParse(input, out PartialAssetId output));
        if (result) Assert.NotEqual(output, default);
    }

    [Theory]
    [InlineData("INVALID_FORMAT")]
    [InlineData("XYZ123")]
    public void CastToUint_InvalidString_ThrowsArgumentException(string input) {
        Assert.Throws<ArgumentException>(() => new PartialAssetId(input));
    }

    [Theory]
    [InlineData("7593F98A", 1972631946u)]
    [InlineData("12345678", 305419896u)]
    [InlineData("1234-5678", 305419896u)]
    [InlineData("0000-162E", 5678u)]
    [InlineData("000-162E", 5678u)]
    [InlineData("00-162E", 5678u)]
    [InlineData("0-162E", 5678u)]
    [InlineData("162E", 5678u)]
    public void CastToUint_ValidString_CorrectConversion(string input, uint expected) {
        var output = new PartialAssetId(input);
        Assert.Equal(expected, output.Value);
    }

    [Theory]
    [InlineData(-12345)]
    public void CastToUint_NegativeNumber_ThrowsArgumentOutOfRange(int input) {
        Assert.Throws<ArgumentOutOfRangeException>(() => new PartialAssetId(input));
    }

    [Theory]
    [InlineData(1234u, "000004D2")]
    [InlineData(5678u, "0000162E")]
    public void ToStringMethod_CorrectFormat(uint input, string expected) {
        var testInstance = new PartialAssetId(input);
        Assert.Equal(expected, testInstance.ToString());
    }

    [Theory]
    [InlineData(1234u, "0000-04D2")]
    [InlineData(5678u, "0000-162E")]
    public void ToStringReadableMethod_CorrectFormat(uint input, string expected) {
        var testInstance = new PartialAssetId(input);
        Assert.Equal(expected, testInstance.ToStringReadable());
    }

    [Theory]
    [InlineData("0000000A", "0000000A", true)]
    [InlineData("0000-000A", "0000-000A", true)]
    [InlineData("0000000A", "0000000B", false)]
    [InlineData("0000-000A", "0000-000B", false)]
    public void PartialAssetIdEqualTest(string x, string y, bool expected) {
        var a = new PartialAssetId(x);
        var b = new PartialAssetId(y);
        Assert.Equal(expected, a.Equals(b));
        Assert.Equal(expected, a == b);
        Assert.Equal(!expected, a != b);
    }

    [Theory]
    [InlineData("0000000A", "000000FF", -1)]
    [InlineData("0000-000A", "0000-00FF", -1)]
    [InlineData("00000100", "000000FF", 1)]
    [InlineData("0000-0100", "0000-00FF", 1)]
    [InlineData("0000000A", "0000000A", 0)]
    [InlineData("0000-000A", "0000-000A", 0)]
    public void ComparableTest(string a, string b, int expected) {
        var pa = new PartialAssetId(a);
        var pb = new PartialAssetId(b);
        Assert.Equal(expected, pa.CompareTo(pb));
    }

    [Theory]
    [InlineData("0000000A", "0000000A", true)]
    [InlineData("0000-000A", "0000-000A", true)]
    [InlineData("0000000A", "0000000B", false)]
    [InlineData("0000-000A", "0000-000B", false)]
    public void GetHashCodeTest(string a, string b, bool shouldEqual) {
        var pa = new PartialAssetId(a);
        var pb = new PartialAssetId(b);

        if (shouldEqual) Assert.Equal(pa.GetHashCode(), pb.GetHashCode());
        else Assert.NotEqual(pa.GetHashCode(), pb.GetHashCode());
    }
}