// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.AterraCore.Common.Types.Nexities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(NameSpace))]
public class NameSpaceTests {
    [Theory]
    [InlineData("ns1")]
    [InlineData("ns1_ns2")]
    [InlineData("ns1-ns2")]
    public void TryCreateNew_ValidString_ReturnsTrue(string value) {
        // Act
        bool result = NameSpace.TryCreateNew(value, out NameSpace? assetName);

        // Assert
        Assert.True(result, $"Expected true for value: {value}, but got false. Regex might be failing to match.");
        Assert.NotNull(assetName);
        Assert.Equal(value, assetName.Value);
        Assert.IsType<NameSpace>(assetName);
    }

    [Theory]
    [InlineData("ns1/")]
    [InlineData("-ns1")]
    [InlineData("_ns2")]
    [InlineData("")]
    public void TryCreateNew_InvalidString_ReturnsFalse(string value) {
        // Act
        bool result = NameSpace.TryCreateNew(value, out NameSpace? assetName);

        // Assert
        Assert.False(result, $"Expected false for value: {value}, but got true. Regex might be incorrectly matching.");
        Assert.Null(assetName);
    }

    [Theory]
    [InlineData("ns1/ns2", "ns1", "ns2")]
    [InlineData("ns1/ns2/ns3", "ns1", "ns2", "ns3")]
    public void Constructor_WithString_InitializesCorrectly(string value, params string[] expectedValues) {
        // Act
        var assetName = new NameSpace(value);

        List<string> valuesList = assetName.Values.ToList();

        // Assert
        Assert.Equal(expectedValues.Length, valuesList.Count);
        for (int i = 0; i < expectedValues.Length; i++) {
            Assert.Equal(expectedValues[i], valuesList[i]);
        }
    }

    [Theory]
    [InlineData("ns1", "ns2")]
    [InlineData("ns1", "ns2", "ns3")]
    public void Constructor_WithIEnumerable_InitializesCorrectly(params string[] values) {
        // Act
        var assetName = new NameSpace(values);

        List<string> valuesList = assetName.Values.ToList();

        // Assert
        Assert.Equal(values.Length, valuesList.Count);
        for (int i = 0; i < values.Length; i++) {
            Assert.Equal(values[i], valuesList[i]);
        }
    }

    [Theory]
    [InlineData("ns1/ns2", "ns1/ns2", true)]
    [InlineData("ns1-ns2", "ns1/ns2", false)]
    [InlineData("ns1/ns2", "ns3/ns4", false)]
    [InlineData("ns1-A/ns2", "ns1-a/ns2", true)]
    [InlineData("alpha", "ALPHA", true)]
    public void EqualityOperators_ShouldWorkCorrectly(string value1, string value2, bool areEqual) {
        // Arrange
        var assetName1 = new NameSpace(value1);
        var assetName2 = new NameSpace(value2);

        // Assert
        Assert.Equal(areEqual, assetName1 == assetName2);
        Assert.Equal(!areEqual, assetName1 != assetName2);
    }

    [Theory]
    [InlineData("ns1/ns2", "ns1/ns2")]
    public void Equals_Method_ShouldWorkCorrectly(string value1, string value2) {
        // Arrange
        var assetName1 = new NameSpace(value1);
        var assetName2 = new NameSpace(value2);

        // Assert
        Assert.True(assetName1.Equals(assetName2));
    }

    [Theory]
    [InlineData("ns1/ns2")]
    [InlineData("ns1-ns2")]
    public void GetHashCode_ShouldBeConsistent(string value) {
        // Arrange
        var assetName1 = new NameSpace(value);
        var assetName2 = new NameSpace(value);

        // Act
        int hashCode1 = assetName1.GetHashCode();
        int hashCode2 = assetName2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    [Theory]
    [InlineData(new[] { "ns1", "ns2" }, "ns1/ns2")]
    [InlineData(new[] { "ns1", "ns2", "ns3" }, "ns1/ns2/ns3")]
    [InlineData(new[] { "ns1-alpha", "ns2", "ns3" }, "ns1-alpha/ns2/ns3")]
    public void ToString_ShouldReturnCorrectFormat(string[] values, string expectedString) {
        // Arrange
        var assetName = new NameSpace(values);

        // Assert
        Assert.Equal(expectedString, assetName.ToString());
    }
}
