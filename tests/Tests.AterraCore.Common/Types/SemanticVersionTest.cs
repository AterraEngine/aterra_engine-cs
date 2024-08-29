// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.AterraCore.Common.Types;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(SemanticVersion))]
public class SemanticVersionTest {
    // Test for constructor with integer parameters and ToString method
    [Theory]
    [InlineData(1, 2, 3, "1.2.3")]
    [InlineData(10, 20, 30, "10.20.30")]
    public void TestSemanticVersionWithIntParams(int major, int minor, int patch, string expected) {
        var semanticVersion = new SemanticVersion(major, minor, patch);
        Assert.Equal(major, semanticVersion.Major);
        Assert.Equal(minor, semanticVersion.Minor);
        Assert.Equal(patch, semanticVersion.Patch);
        Assert.Equal(expected, semanticVersion.ToString());
    }

    // Test for constructor with string parameter
    [Theory]
    [InlineData("1.2.3", 1, 2, 3)]
    [InlineData("10.20.30", 10, 20, 30)]
    public void TestSemanticVersionConstructorWithStringParam_ValidString(string version, int major, int minor, int patch) {
        var semanticVersion = new SemanticVersion(version);
        Assert.Equal(major, semanticVersion.Major);
        Assert.Equal(minor, semanticVersion.Minor);
        Assert.Equal(patch, semanticVersion.Patch);
    }

    // Test for constructor with string parameter & addendum
    [Theory]
    [InlineData("1.2.3-alpha", 1, 2, 3, "alpha")]
    [InlineData("10.20.30-beta", 10, 20, 30, "beta")]
    [InlineData("10.20.30-beta_gamma", 10, 20, 30, "beta_gamma")]
    public void TestSemanticVersionConstructorWithStringParamAddendum_ValidString(string version, int major, int minor, int patch, string addendum) {
        var semanticVersion = new SemanticVersion(version);
        Assert.Equal(major, semanticVersion.Major);
        Assert.Equal(minor, semanticVersion.Minor);
        Assert.Equal(patch, semanticVersion.Patch);
        Assert.Equal(addendum, semanticVersion.Addendum);
    }

    // Test for constructor with string parameters (leading zeroes)
    [Theory]
    [InlineData("01.002.0003", 1, 2, 3)]
    public void TestSemanticVersionConstructorWithStringLeadingZeroes_ValidString(string version, int major, int minor, int patch) {
        var semanticVersion = new SemanticVersion(version);
        Assert.Equal(major, semanticVersion.Major);
        Assert.Equal(minor, semanticVersion.Minor);
        Assert.Equal(patch, semanticVersion.Patch);
    }

    // Test for comparison of versions with and without addendum
    [Theory]
    [InlineData("1.2.3-alpha", "1.2.3")]
    public void TestSemanticVersionComparisonWithWithoutAddendum(string versionWithAddendum, string versionWithoutAddendum) {
        var semanticVersionWithAddendum = new SemanticVersion(versionWithAddendum);
        var semanticVersionWithoutAddendum = new SemanticVersion(versionWithoutAddendum);
        // Depending upon your requirements, the version with Addendum might be considered lesser, greater or equal
        Assert.True(semanticVersionWithoutAddendum < semanticVersionWithAddendum);
    }

    // Test for constructor with invalid string parameter
    [Theory]
    [InlineData("1.2.3.4")]
    [InlineData("1.2")]
    [InlineData("abcd")]
    public void TestSemanticVersionConstructorWithStringParam_InvalidString(string version) {
        Assert.Throws<ArgumentException>(() => new SemanticVersion(version));
    }

    // Test for constructor with string parameter & addendum
    [Theory]
    [InlineData("1.2.3-alpha-")]
    [InlineData("10.20.30-beta-a")]
    public void TestSemanticVersionConstructorWithStringParamAddendum_InvalidString(string version) {
        Assert.Throws<ArgumentException>(() => new SemanticVersion(version));
    }

    // Test for equality (==)
    [Theory]
    [InlineData(1, 1, 1, 1, 1, 1)]
    public void TestEqualityOperator(int majorA, int minorA, int patchA, int majorB, int minorB, int patchB) {
        SemanticVersion semanticVersionA = new(majorA, minorA, patchA);
        SemanticVersion semanticVersionB = new(majorB, minorB, patchB);

        Assert.True(semanticVersionA == semanticVersionB);
    }

    // Test for equality (!=)
    [Theory]
    [InlineData(1, 1, 1, 0, 0, 0)]
    [InlineData(1, 1, 1, 0, 1, 0)]
    [InlineData(1, 1, 1, 0, 0, 1)]
    public void TestInEqualityOperator(int majorA, int minorA, int patchA, int majorB, int minorB, int patchB) {
        SemanticVersion semanticVersionA = new(majorA, minorA, patchA);
        SemanticVersion semanticVersionB = new(majorB, minorB, patchB);

        Assert.True(semanticVersionA != semanticVersionB);
    }

    // Test for less than (<)
    [Theory]
    [InlineData(1, 0, 0, 2, 0, 0)]
    [InlineData(2, 1, 0, 2, 2, 0)]
    [InlineData(2, 2, 1, 2, 2, 2)]
    public void TestLessThanOperator(int majorA, int minorA, int patchA, int majorB, int minorB, int patchB) {
        var versionA = new SemanticVersion(majorA, minorA, patchA);
        var versionB = new SemanticVersion(majorB, minorB, patchB);
        Assert.True(versionA < versionB);
    }

    // Test for greater than (>)
    [Theory]
    [InlineData(2, 0, 0, 1, 0, 0)]
    [InlineData(2, 2, 0, 2, 1, 0)]
    [InlineData(2, 2, 2, 2, 2, 1)]
    public void TestGreaterThanOperator(int majorA, int minorA, int patchA, int majorB, int minorB, int patchB) {
        var versionA = new SemanticVersion(majorA, minorA, patchA);
        var versionB = new SemanticVersion(majorB, minorB, patchB);
        Assert.True(versionA > versionB);
    }
    // Test for CompareTo
    [Theory]
    [InlineData(1, 2, 3, 1, 2, 4, -1)]
    [InlineData(1, 2, 3, 1, 2, 2, 1)]
    [InlineData(1, 2, 3, 1, 2, 3, 0)]
    public void TestCompareTo(int majorA, int minorA, int patchA, int majorB, int minorB, int patchB, int expected) {
        var versionA = new SemanticVersion(majorA, minorA, patchA);
        var versionB = new SemanticVersion(majorB, minorB, patchB);
        Assert.Equal(expected, versionA.CompareTo(versionB));
    }
    
    // Test for CompareTo method with Addendum comparison
    [Theory]
    [InlineData(1, 2, 3, "alpha", 1, 2, 3, null, 1)]
    [InlineData(1, 2, 3, null, 1, 2, 3, "alpha", -1)]
    [InlineData(1, 2, 3, "alpha", 1, 2, 3, "alpha", 0)]
    public void TestCompareToWithAddendum(int majorA, int minorA, int patchA, string? addendumA, int majorB, int minorB, int patchB, string? addendumB, int expected)
    {
        var versionA = new SemanticVersion(majorA, minorA, patchA, addendumA);
        var versionB = new SemanticVersion(majorB, minorB, patchB, addendumB);
        Assert.Equal(expected, versionA.CompareTo(versionB));
    }
    
    
    // Test for implicit string conversion
    [Theory]
    [InlineData("1.2.3", 1, 2, 3)]
    public void TestImplicitStringConversion(string versionString, int major, int minor, int patch) {
        SemanticVersion version = versionString;
        Assert.Equal(major, version.Major);
        Assert.Equal(minor, version.Minor);
        Assert.Equal(patch, version.Patch);
    }

    // Test for implicit SemanticVersion conversion
    [Theory]
    [InlineData(1, 2, 3, "1.2.3")]
    public void TestImplicitSemanticVersionConversion(int major, int minor, int patch, string expected) {
        var version = new SemanticVersion(major, minor, patch);
        string versionString = version;
        Assert.Equal(expected, versionString);
    }

    // Test for TryParse
    [Theory]
    [InlineData("1.2.3", true, 1, 2, 3)]
    [InlineData("invalid", false, 0, 0, 0)]
    public void TestTryParse(string input, bool expectedResult, int expectedMajor, int expectedMinor, int expectedPatch) {
        bool result = SemanticVersion.TryParse(input, out var version);
        Assert.Equal(expectedResult, result);
        if (expectedResult) {
            Assert.Equal(expectedMajor, version.Major);
            Assert.Equal(expectedMinor, version.Minor);
            Assert.Equal(expectedPatch, version.Patch);
        }
        else {
            Assert.Equal(SemanticVersion.Zero, version);
        }
    }

    // Test for Equals
    [Theory]
    [InlineData(1, 1, 1, true)]
    [InlineData(1, 2, 3, false)]
    public void TestEquals(int major, int minor, int patch, bool expected) {
        var versionA = new SemanticVersion(1, 1, 1);
        var versionB = new SemanticVersion(major, minor, patch);
        Assert.Equal(expected, versionA.Equals(versionB));
    }

    // Test for GetHashCode
    [Fact]
    public void TestGetHashCode() {
        var versionA = new SemanticVersion(1, 2, 3);
        var versionB = new SemanticVersion(1, 2, 3);
        Assert.Equal(versionA.GetHashCode(), versionB.GetHashCode());
    }

    // Test for Default Constructor
    [Fact]
    public void TestDefaultConstructor() {
        var version = new SemanticVersion();
        Assert.Equal(0, version.Major);
        Assert.Equal(0, version.Minor);
        Assert.Equal(0, version.Patch);
        Assert.Null(version.Addendum);
    }
    
    // Test for Equals(object)
    [Theory]
    [InlineData(1, 2, 3, true)]
    [InlineData(1, 2, 4, false)]
    public void TestObjectEquals(int major, int minor, int patch, bool expected) {
        var versionA = new SemanticVersion(1, 2, 3);
        // ReSharper disable once RedundantCast
        object versionB = (object)new SemanticVersion(major, minor, patch);
        Assert.Equal(expected, versionA.Equals(versionB));
    }

    // Test for less than or equal operator (<=)
    [Theory]
    [InlineData(1, 2, 3, 1, 2, 3, true)]
    [InlineData(1, 2, 3, 1, 2, 4, true)]
    [InlineData(1, 2, 4, 1, 2, 3, false)]
    public void TestLessThanOrEqualOperator(int majorA, int minorA, int patchA, int majorB, int minorB, int patchB, bool expected)
    {
        var versionA = new SemanticVersion(majorA, minorA, patchA);
        var versionB = new SemanticVersion(majorB, minorB, patchB);
        Assert.Equal(expected, versionA <= versionB);
    }

    // Test for greater than or equal operator (>=)
    [Theory]
    [InlineData(1, 2, 3, 1, 2, 3, true)]
    [InlineData(1, 2, 4, 1, 2, 3, true)]
    [InlineData(1, 2, 3, 1, 2, 4, false)]
    public void TestGreaterThanOrEqualOperator(int majorA, int minorA, int patchA, int majorB, int minorB, int patchB, bool expected)
    {
        var versionA = new SemanticVersion(majorA, minorA, patchA);
        var versionB = new SemanticVersion(majorB, minorB, patchB);
        Assert.Equal(expected, versionA >= versionB);
    }

    [Fact]
    public void TestMax() {
        SemanticVersion max = SemanticVersion.Max;
        Assert.Equal(int.MaxValue, max.Major);
        Assert.Equal(int.MaxValue, max.Minor);
        Assert.Equal(int.MaxValue, max.Patch);
        Assert.Equal(string.Empty, max.Addendum);
    }
    
    [Fact]
    public void TestZero() {
        SemanticVersion zeo = SemanticVersion.Zero;
        Assert.Equal(0, zeo.Major);
        Assert.Equal(0, zeo.Minor);
        Assert.Equal(0, zeo.Patch);
        Assert.Equal(string.Empty, zeo.Addendum);
    }
}
