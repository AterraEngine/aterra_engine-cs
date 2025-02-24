// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using JetBrains.Annotations;

namespace Tests.AterraEngine.Common.Frameworks.Omnia.Types;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(AssetNameSpace))]
public class AssetNameSpaceTest {

    [Test]
    [Arguments("plugin1")]
    [Arguments("plugin_name")]
    [Arguments("plugin-name")]
    public async Task TryCreateNew_ValidString_ReturnsTrue(string value) {
        // Arrange & Act
        bool result = AssetNameSpace.TryCreateNew(value, out AssetNameSpace? pluginId);

        // Assert
        await Assert.That(result).IsTrue().Because($"Expected true for value: {value}, but got false. Regex might be failing to match.");
        await Assert.That(pluginId).IsNotNull();
        await Assert.That(pluginId!.Value).IsEqualTo(value);
    }

    [Test]
    [Arguments("plugin1/")]
    [Arguments("-plugin1")]
    [Arguments("_plugin")]
    [Arguments("")]
    [Arguments("plugin1//plugin2")]
    [Arguments("plugin1/.plugin2")]
    [Arguments("plugin1_")]
    [Arguments("plugin1-")]
    [Arguments("/plugin2")]
    [Arguments("plugin.name")]
    [Arguments("plugin1/plugin2")]
    public async Task TryCreateNew_InvalidString_ReturnsFalse(string value) {
        // Arrange & Act
        bool result = AssetNameSpace.TryCreateNew(value, out AssetNameSpace? pluginId);

        // Assert
        await Assert.That(result).IsFalse().Because($"Expected false for value: {value}, but got true. Regex might be failing to match.");
        await Assert.That(pluginId).IsNull();
    }

    [Test]
    public async Task Constructor_WithString_InitializesCorrectly() {
        // Arrange
        const string value = "plugin1";

        // Act
        var pluginId = new AssetNameSpace(value);

        // Assert
        await Assert.That(pluginId).IsNotNull();
    }

    [Test]
    public async Task Constructor_ThrowsArgumentException_ForInvalidString() {
        // Arrange & Act
        const string value = "plugin1/";

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(() => Task.FromResult(new AssetNameSpace(value)));
    }

    [Test]
    public async Task Equals_ShouldReturnTrue_ForSameValue() {
        // Arrange & Act
        var pluginId1 = new AssetNameSpace("plugin1");
        var pluginId2 = new AssetNameSpace("plugin1");

        // Assert
        await Assert.That(pluginId1).IsEqualTo(pluginId2);
    }

    [Test]
    public async Task Equals_ShouldReturnFalse_ForDifferentValue() {
        // Arrange & Act
        var pluginId1 = new AssetNameSpace("plugin1");
        var pluginId2 = new AssetNameSpace("plugin2");

        // Assert
        await Assert.That(pluginId1).IsNotEqualTo(pluginId2);
    }

    [Test]
    public async Task EqualityOperators_ShouldWorkCorrectly() {
        // Arrange & Act
        var pluginId1 = new AssetNameSpace("plugin1");
        var pluginId2 = new AssetNameSpace("plugin1");
        var pluginId3 = new AssetNameSpace("plugin2");

        // Assert
        await Assert.That(pluginId1).IsEqualTo(pluginId2)
            .And.IsNotEqualTo(pluginId3);
    }

    [Test]
    public async Task ToString_ShouldReturnCorrectFormat() {
        // Arrange
        var pluginId = new AssetNameSpace("plugin1");

        // Act
        string result = pluginId.ToString();

        // Assert
        await Assert.That(result).IsEqualTo("plugin1");
    }

    [Test]
    public async Task GetHashCode_ShouldBeConsistent() {
        // Arrange
        var pluginId1 = new AssetNameSpace("plugin1");
        var pluginId2 = new AssetNameSpace("plugin1");

        // Act
        int hashCode1 = pluginId1.GetHashCode();
        int hashCode2 = pluginId2.GetHashCode();

        // Assert
        await Assert.That(hashCode1).IsEqualTo(hashCode2);
    }

    [Test]
    public async Task AdditionOperator_ShouldCombineAssetNameSpaceAndAssetName() {
        // Arrange
        var pluginId = new AssetNameSpace("plugin1");
        var nameSpace = new AssetPath("asset1");

        // Act
        OmniaId assetId = pluginId + nameSpace;

        // Assert
        await Assert.That(assetId).IsEqualTo("plugin1:asset1");
    }
}
