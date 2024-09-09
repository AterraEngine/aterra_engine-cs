// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.AterraCore.Common.Types.Nexities;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(AssetTag))]
public class AssetTagTest {
    [Theory]
    [InlineData("#pluginName:folder/item", "pluginName", new[] { "folder", "item" })]
    [InlineData("#plugin_Name:folder/item", "plugin_Name", new[] { "folder", "item" })]
    [InlineData("#plugin_Name:folder_another/item", "plugin_Name", new[] { "folder_another", "item" })]
    [InlineData("#pluginName:folder_another/item", "pluginName", new[] { "folder_another", "item" })]
    [InlineData("#PLUGINNAME:FOLDER/ITEM", "PLUGINNAME", new[] { "FOLDER", "ITEM" })]
    [InlineData("#PLUGIN_NAME:FOLDER/ITEM", "PLUGIN_NAME", new[] { "FOLDER", "ITEM" })]
    [InlineData("#PLUGIN_NAME:FOLDER_ANOTHER/ITEM", "PLUGIN_NAME", new[] { "FOLDER_ANOTHER", "ITEM" })]
    [InlineData("#PLUGINNAME:FOLDER_ANOTHER/ITEM", "PLUGINNAME", new[] { "FOLDER_ANOTHER", "ITEM" })]
    public void AssetTagCreationTest(string fullAssetTag, string pluginName, string[] namespaces) {
        var assetIdRegex = new AssetTag(fullAssetTag);
        Assert.Equal(pluginName, assetIdRegex.PluginId.Value);
        Assert.Equal(namespaces, assetIdRegex.NameSpace.Values);
    }

    [Theory]
    [InlineData("pluginName", "folder/item", "pluginName", new[] { "folder", "item" })]
    [InlineData("plugin_Name", "folder/item", "plugin_Name", new[] { "folder", "item" })]
    [InlineData("plugin_Name", "folder_another/item", "plugin_Name", new[] { "folder_another", "item" })]
    [InlineData("pluginName", "folder_another/item", "pluginName", new[] { "folder_another", "item" })]
    [InlineData("PLUGINNAME", "FOLDER/ITEM", "PLUGINNAME", new[] { "FOLDER", "ITEM" })]
    [InlineData("PLUGIN_NAME", "FOLDER/ITEM", "PLUGIN_NAME", new[] { "FOLDER", "ITEM" })]
    [InlineData("PLUGIN_NAME", "FOLDER_ANOTHER/ITEM", "PLUGIN_NAME", new[] { "FOLDER_ANOTHER", "ITEM" })]
    [InlineData("PLUGINNAME", "FOLDER_ANOTHER/ITEM", "PLUGINNAME", new[] { "FOLDER_ANOTHER", "ITEM" })]
    public void AssetTagThoughFullStringsCreationTest(string pluginId, string assetName, string pluginName, string[] namespaces) {
        var assetIdRegex = new AssetTag(pluginId, assetName);
        Assert.Equal(pluginName, assetIdRegex.PluginId.Value);
        Assert.Equal(namespaces, assetIdRegex.NameSpace.Values);
    }


    [Theory]
    [InlineData("pluginName", new[] { "folder", "item" }, "pluginName", new[] { "folder", "item" })]
    [InlineData("plugin_Name", new[] { "folder", "item" }, "plugin_Name", new[] { "folder", "item" })]
    [InlineData("plugin_Name", new[] { "folder_another", "item" }, "plugin_Name", new[] { "folder_another", "item" })]
    [InlineData("pluginName", new[] { "folder_another", "item" }, "pluginName", new[] { "folder_another", "item" })]
    [InlineData("PLUGINNAME", new[] { "FOLDER", "ITEM" }, "PLUGINNAME", new[] { "FOLDER", "ITEM" })]
    [InlineData("PLUGIN_NAME", new[] { "FOLDER", "ITEM" }, "PLUGIN_NAME", new[] { "FOLDER", "ITEM" })]
    [InlineData("PLUGIN_NAME", new[] { "FOLDER_ANOTHER", "ITEM" }, "PLUGIN_NAME", new[] { "FOLDER_ANOTHER", "ITEM" })]
    [InlineData("PLUGINNAME", new[] { "FOLDER_ANOTHER", "ITEM" }, "PLUGINNAME", new[] { "FOLDER_ANOTHER", "ITEM" })]
    [InlineData("PluginName", new[] { "Folder", "Item" }, "PluginName", new[] { "Folder", "Item" })]
    public void AssetTagThoughStringPartsCreationTest(string pluginId, IEnumerable<string> assetName, string pluginName, string[] namespaces) {
        var assetIdRegex = new AssetTag(pluginId, assetName);
        Assert.Equal(pluginName, assetIdRegex.PluginId.Value);
        Assert.Equal(namespaces, assetIdRegex.NameSpace.Values);
    }

    [Theory]
    [InlineData("#pluginName:folder/")]
    [InlineData("#pluginName:folder-")]
    [InlineData("#pluginName:folder_")]
    [InlineData("#pluginName:")]
    [InlineData("#pluginName/folder/")]
    [InlineData("#pluginName_:alpha")]
    [InlineData("#pluginName-:alpha")]
    [InlineData("#PluginName:Folder.Item")]
    [InlineData("#PLUGINNAME:FOLDER_ANOTHER.ITEM")]
    public void AssetTagCreateThrowsTest(string input) {
        Assert.Throws<ArgumentException>(() => new AssetTag(input));
    }

    [Theory]
    [InlineData("#pluginName:folder/")]
    [InlineData("#pluginName:folder-")]
    [InlineData("#pluginName:folder_")]
    [InlineData("#pluginName:")]
    [InlineData("#pluginName/folder/")]
    [InlineData("#pluginName_:alpha")]
    [InlineData("#pluginName-:alpha")]
    [InlineData("#PluginName:Folder.Item")]
    [InlineData(@"#pluginName:folder\")]
    [InlineData(@"#pluginName\folder\")]
    public void AssetTagCreateFailsTest(string input) {
        Assert.False(AssetTag.TryCreateNew(input, out AssetTag? output));
        Assert.Null(output);
    }


    [Theory]
    [InlineData("#pluginName:folder/item", "#PLUGINNAME:FOLDER/ITEM")]
    public void AssetTagEqualityTest(string a, string b) {
        var assetA = new AssetTag(a);
        var assetB = new AssetTag(b);

        Assert.Equal(assetA, assetB);
    }

    [Theory]
    [InlineData("#pluginName:folder/item", "#PLUGINNAME:FOLDER/other")]
    public void AssetTag_EqualityFail_Test(string a, string b) {
        var assetA = new AssetTag(a);
        var assetB = new AssetTag(b);

        Assert.NotEqual(assetA, assetB);
    }


    [Theory]
    [InlineData("#pluginName:folder/item", "pluginName", "folder/item")]
    public void AssetTagCreationThroughStructsTest(string result, string pluginId, string assetName) {
        var newAsset = new AssetTag(new PluginId(pluginId), new NameSpace(assetName));

        Assert.Equal(result, newAsset.ToString());
    }
}
