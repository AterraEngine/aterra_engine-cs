// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.AterraCore.Common.Types.Nexities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(PluginId))]
public class PluginIdTest {
    [Theory]
    [InlineData("plugin1")]
    [InlineData("plugin_name")]
    [InlineData("plugin-name")]
    public void TryCreateNew_ValidString_ReturnsTrue(string value) {
        bool result = PluginId.TryCreateNew(value, out PluginId? pluginId);

        Assert.True(result, $"Expected true for value: {value}, but got false. Regex might be failing to match.");
        Assert.NotNull(pluginId);
        Assert.Equal(value, pluginId.Value);
        Assert.IsType<PluginId>(pluginId);
    }

    [Theory]
    [InlineData("plugin1/")]
    [InlineData("-plugin1")]
    [InlineData("_plugin")]
    [InlineData("")]
    [InlineData("plugin1//plugin2")]
    [InlineData("plugin1/.plugin2")]
    [InlineData("plugin1_")]
    [InlineData("plugin1-")]
    [InlineData("/plugin2")]
    [InlineData("plugin.name")]
    [InlineData("plugin1/plugin2")]
    public void TryCreateNew_InvalidString_ReturnsFalse(string value) {
        bool result = PluginId.TryCreateNew(value, out PluginId? pluginId);

        Assert.False(result, $"Expected false for value: {value}, but got true. Regex might be incorrectly matching.");
        Assert.Null(pluginId);
    }

    [Fact]
    public void Constructor_WithString_InitializesCorrectly() {
        string value = "plugin1";

        var pluginId = new PluginId(value);

        Assert.Equal(value, pluginId.Value);
    }

    [Fact]
    public void Constructor_ThrowsArgumentException_ForInvalidString() {
        string value = "plugin1/";

        Assert.Throws<ArgumentException>(() => new PluginId(value));
    }

    [Fact]
    public void Equals_ShouldReturnTrue_ForSameValue() {
        var pluginId1 = new PluginId("plugin1");
        var pluginId2 = new PluginId("plugin1");

        Assert.True(pluginId1.Equals(pluginId2));
    }

    [Fact]
    public void Equals_ShouldReturnFalse_ForDifferentValue() {
        var pluginId1 = new PluginId("plugin1");
        var pluginId2 = new PluginId("plugin2");

        Assert.False(pluginId1.Equals(pluginId2));
    }

    [Fact]
    public void EqualityOperators_ShouldWorkCorrectly() {
        var pluginId1 = new PluginId("plugin1");
        var pluginId2 = new PluginId("plugin1");
        var pluginId3 = new PluginId("plugin2");

        Assert.True(pluginId1 == pluginId2);
        Assert.False(pluginId1 != pluginId2);
        Assert.True(pluginId1 != pluginId3);
    }

    [Fact]
    public void ToString_ShouldReturnCorrectFormat() {
        var pluginId = new PluginId("plugin1");

        string result = pluginId.ToString();

        Assert.Equal("plugin1", result);
    }

    [Fact]
    public void GetHashCode_ShouldBeConsistent() {
        var pluginId1 = new PluginId("plugin1");
        var pluginId2 = new PluginId("plugin1");

        int hashCode1 = pluginId1.GetHashCode();
        int hashCode2 = pluginId2.GetHashCode();

        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void AdditionOperator_ShouldCombinePluginIdAndAssetName() {
        var pluginId = new PluginId("plugin1");
        var nameSpace = new NameSpace("asset1");

        AssetId assetId = pluginId + nameSpace;

        Assert.Equal("plugin1:asset1", assetId);
    }
}
