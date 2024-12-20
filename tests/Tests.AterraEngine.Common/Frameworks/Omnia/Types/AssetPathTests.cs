// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using JetBrains.Annotations;

namespace Tests.AterraEngine.Common.Frameworks.Omnia.Types;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(AssetPath))]
public class AssetPathTests {
    [Test]
    [Arguments("ns1")]
    [Arguments("ns1_ns2")]
    [Arguments("ns1-ns2")]
    public async Task TryCreateNew_ValidString_ReturnsTrue(string value) {
        // Arrange & Act
        bool result = AssetPath.TryCreateNew(value, out AssetPath? assetName);

        // Assert
        await Assert.That(result).IsTrue().Because($"Expected true for value: {value}, but got false. Regex might be failing to match.");
        await Assert.That(assetName).IsNotNull();
        await Assert.That(assetName!.Value).IsEqualTo(value);
        await Assert.That(assetName).IsTypeOf<AssetPath>();
    }

    [Test]
    [Arguments("ns1/")]
    [Arguments("-ns1")]
    [Arguments("_ns2")]
    [Arguments("")]
    public async Task TryCreateNew_InvalidString_ReturnsFalse(string value) {
        // Arrange & Act
        bool result = AssetPath.TryCreateNew(value, out AssetPath? assetName);

        // Assert
        await Assert.That(assetName).IsNull();
        await Assert.That(result).IsFalse().Because($"Expected false for value: {value}, but got true. Regex might be incorrectly matching.");
    }

    [Test]
    [Arguments("ns1/ns2", new[] { "ns1", "ns2" })]
    [Arguments("ns1/ns2/ns3", new [] {"ns1", "ns2", "ns3"})]
    public async Task Constructor_WithString_InitializesCorrectly(string value, params string[] expectedValues) {
        // Arrange
        var assetName = new AssetPath(value);

        // Act
        List<string> valuesList = assetName.Values.ToList();

        // Assert
        await Assert.That(valuesList.Count).IsEqualTo(expectedValues.Length);
        for (int i = 0; i < expectedValues.Length; i++) {
            await Assert.That(valuesList[i]).IsEqualTo(expectedValues[i]);
        }
    }

    [Test]
    public async Task Constructor_WithIEnumerable_InitializesCorrectly(
        [Matrix("a", "b", "c", "d", null)] string? path1,
        [Matrix("a", "b", "c", null, "d")] string? path2,
        [Matrix("a", "b", null, "c", "d")] string? path3,
        [Matrix("a", null, "b", "c", "d")] string? path4,
        [Matrix("a", "b", "c", "d","e")] string? path5 // else there is an error where all paths are null
    ) {
        // Arrange
        var assetName = new AssetPath(path1, path2, path3 , path4, path5);
    
        // Act
        List<string> valuesList = assetName.Values.ToList();
    
        // Assert
        await Assert.That(valuesList.Count).IsEqualTo(4);
        await Assert.That(valuesList[0]).IsEqualTo(path1);
        await Assert.That(valuesList[1]).IsEqualTo(path2);
        await Assert.That(valuesList[2]).IsEqualTo(path3);
        await Assert.That(valuesList[3]).IsEqualTo(path4);
        await Assert.That(valuesList[4]).IsEqualTo(path5);
    }

    [Test]
    [Arguments("ns1/ns2", "ns1/ns2", true)]
    [Arguments("ns1-ns2", "ns1/ns2", false)]
    [Arguments("ns1/ns2", "ns3/ns4", false)]
    [Arguments("ns1-A/ns2", "ns1-a/ns2", true)]
    [Arguments("alpha", "ALPHA", true)]
    public async Task EqualityOperators_ShouldWorkCorrectly(string value1, string value2, bool areEqual) {
        // Arrange
        var assetName1 = new AssetPath(value1);
        var assetName2 = new AssetPath(value2);

        // Act
        bool result = assetName1 == assetName2;
        bool result2 = assetName1 != assetName2;
        
        // Assert
        await Assert.That(result).IsEqualTo(areEqual);
        await Assert.That(result2).IsEqualTo(!areEqual);
    }

    [Test]
    [Arguments("ns1/ns2", "ns1/ns2")]
    public async Task Equals_Method_ShouldWorkCorrectly(string value1, string value2) {
        // Arrange
        var assetName1 = new AssetPath(value1);
        var assetName2 = new AssetPath(value2);
        
        // Act
        bool result = assetName1.Equals(assetName2);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    [Arguments("ns1/ns2")]
    [Arguments("ns1-ns2")]
    public async Task GetHashCode_ShouldBeConsistent(string value) {
        // Arrange
        var assetName1 = new AssetPath(value);
        var assetName2 = new AssetPath(value);

        // Act
        int hashCode1 = assetName1.GetHashCode();
        int hashCode2 = assetName2.GetHashCode();

        // Assert
        await Assert.That(hashCode1).IsEqualTo(hashCode2);
    }

    [Test]
    [Arguments(new[] { "ns1", "ns2" }, "ns1/ns2")]
    [Arguments(new[] { "ns1", "ns2", "ns3" }, "ns1/ns2/ns3")]
    [Arguments(new[] { "ns1-alpha", "ns2", "ns3" }, "ns1-alpha/ns2/ns3")]
    public async Task ToString_ShouldReturnCorrectFormat(string[] values, string expectedString) {
        // Arrange
        var assetName = new AssetPath(values);
        
        // Act
        string result = assetName.ToString();

        // Assert
        await Assert.That(result).IsEqualTo(expectedString);
    }
}
