// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using JetBrains.Annotations;

namespace AterraEngine.Tests.Core.Types.Types.Nexities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(AssetId))]
public class AssetIdTest {
    [Theory]
    [InlineData("pluginName:folder/item", "pluginName", new[] { "folder", "item" })]
    [InlineData("plugin_Name:folder/item", "plugin_Name", new[] { "folder", "item" })]
    [InlineData("plugin_Name:folder_another/item", "plugin_Name", new[] { "folder_another", "item" })]
    [InlineData("pluginName:folder_another/item", "pluginName", new[] { "folder_another", "item" })]
    [InlineData("PLUGINNAME:FOLDER/ITEM", "PLUGINNAME", new[] { "FOLDER", "ITEM" })]
    [InlineData("PLUGIN_NAME:FOLDER/ITEM", "PLUGIN_NAME", new[] { "FOLDER", "ITEM" })]
    [InlineData("PLUGIN_NAME:FOLDER_ANOTHER/ITEM", "PLUGIN_NAME", new[] { "FOLDER_ANOTHER", "ITEM" })]
    [InlineData("PLUGINNAME:FOLDER_ANOTHER/ITEM", "PLUGINNAME", new[] { "FOLDER_ANOTHER", "ITEM" })]
    public void AssetId_Creation_Test(string fullAssetId, string pluginName, string[] namespaces) {
        var assetIdRegex = new AssetId(fullAssetId);
        Assert.Equal(pluginName,assetIdRegex.PluginId.Value);
        Assert.Equal(namespaces,assetIdRegex.AssetName.Values);
    }
    
    [Theory]
    [InlineData("pluginName:folder/")]
    [InlineData("pluginName:folder-")]
    [InlineData("pluginName:folder_")]
    [InlineData("pluginName:")]
    [InlineData("pluginName/folder/")]
    [InlineData("pluginName_:alpha")]
    [InlineData("pluginName-:alpha")]
    public void AssetId_CreateThrows_Test(string input) {
        Assert.Throws<ArgumentException>(() => new AssetId(input));
    }
    
    [Theory]
    [InlineData("pluginName:folder/")]
    [InlineData("pluginName:folder-")]
    [InlineData("pluginName:folder_")]
    [InlineData("pluginName:")]
    [InlineData("pluginName/folder/")]
    [InlineData("pluginName_:alpha")]
    [InlineData("pluginName-:alpha")]
    public void AssetId_CreateFails_Test(string input) {
        Assert.False(AssetId.TryCreateNew(input, out AssetId? output));
        Assert.Null(output);
    }


    [Theory]
    [InlineData("pluginName:folder/item", "PLUGINNAME:FOLDER/ITEM")]
    public void AssetId_Equality_Test(string a, string b) {
        var assetA = new AssetId(a);
        var assetB = new AssetId(b);
        
        Assert.Equal(assetA, assetB);
    }
    
    [Theory]
    [InlineData("pluginName:folder/item", "PLUGINNAME:FOLDER/other")]
    public void AssetId_EqualityFail_Test(string a, string b) {
        var assetA = new AssetId(a);
        var assetB = new AssetId(b);
        
        Assert.NotEqual(assetA, assetB);
    }
    
    
    [Theory]
    [InlineData("pluginName:folder/item", "pluginName", "folder/item" )]
    public void AssetId_Union_Test(string result, string pluginId, string assetName) {
        var left = new PluginId(pluginId);
        var right = new AssetName(assetName);
        AssetId newAsset = left | right;
        
        Assert.Equal(result, newAsset.ToString());
    }
}