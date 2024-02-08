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

    [Fact]
    public void TestConstructorFromPluginIdAndValue() {
        var pluginId = new PluginId(100);
        var assetId = new AssetId(pluginId, 1);

        Assert.Equal(pluginId, assetId.PluginId);
        Assert.Equal(1u, assetId.Id);
    }

    [Fact]
    public void TestConstructorFromValue() {
        var assetId = new AssetId(1);
        Assert.Equal(new PluginId(0), assetId.PluginId);
        Assert.Equal(1u, assetId.Id);
    }

    [Fact]
    public void TestConstructorFromString() {
        var assetId = new AssetId("00000001");
        Assert.Equal(new PluginId(0), assetId.PluginId);
        Assert.Equal(1u, assetId.Id);
        
        var assetId2 = new AssetId("0001-00000001");
        Assert.Equal(new PluginId(1), assetId2.PluginId);
        Assert.Equal(1u, assetId2.Id);
        
        
        var assetId3 = new AssetId("1");
        Assert.Equal(new PluginId(0), assetId3.PluginId);
        Assert.Equal(1u, assetId3.Id);
    }
    

    [Fact]
    public void TestConstructorFromStringWithInvalidValue() {
        Assert.Throws<ArgumentException>(() => new AssetId("0000-000100020003"));
    }

    [Fact]
    public void TestToString() {
        var expectedToString = "000000000001";
        var assetId = new AssetId(1);

        Assert.Equal(expectedToString, assetId.ToString());
    }

    [Fact]
    public void TestToStringReadable() {
        var expectedToString = "0000-00000001";
        var assetId = new AssetId(1);

        Assert.Equal(expectedToString, assetId.ToStringReadable());
    }

    [Fact]
    public void TestCompareTo() {
        var assetId1 = new AssetId(1);
        var assetId2 = new AssetId(2);

        Assert.True(assetId1.CompareTo(assetId2) < 0);
        Assert.True(assetId2.CompareTo(assetId1) > 0);
    }

    [Fact]
    public void TestEquals_Operator() {
        var assetId1 = new AssetId(1);
        var assetId2 = new AssetId(1);

        Assert.True(assetId1 == assetId2);
    }

    [Fact]
    public void TestNotEquals_Operator() {
        var assetId1 = new AssetId(1);
        var assetId2 = new AssetId(2);

        Assert.True(assetId1 != assetId2);
    }

    [Fact]
    public void TestEquals() {
        var assetId1 = new AssetId(1);
        var assetId2 = new AssetId(1);

        Assert.True(assetId1.Equals(assetId2));
    }

    [Fact]
    public void TestEquals_Object() {
        var assetId1 = new AssetId(1);
        object assetId2 = new AssetId(1);

        Assert.True(assetId1.Equals(assetId2));
    }

    [Fact]
    public void TestGetHashCode() {
        var assetId1 = new AssetId(1);
        var assetId2 = new AssetId(1);

        Assert.Equal(assetId1.GetHashCode(), assetId2.GetHashCode());
    }
}