// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using JetBrains.Annotations;

namespace Tests.AterraEngine.Common.Frameworks.Omnia.Types;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(OmniaId))]
public class OmniaIdTest {
    
    [Test]
    [Arguments("pluginName:folder/item", "pluginName", new[] { "folder", "item" })]
    [Arguments("plugin_Name:folder/item", "plugin_Name", new[] { "folder", "item" })]
    [Arguments("plugin_Name:folder_another/item", "plugin_Name", new[] { "folder_another", "item" })]
    [Arguments("pluginName:folder_another/item", "pluginName", new[] { "folder_another", "item" })]
    [Arguments("PLUGINNAME:FOLDER/ITEM", "PLUGINNAME", new[] { "FOLDER", "ITEM" })]
    [Arguments("PLUGIN_NAME:FOLDER/ITEM", "PLUGIN_NAME", new[] { "FOLDER", "ITEM" })]
    [Arguments("PLUGIN_NAME:FOLDER_ANOTHER/ITEM", "PLUGIN_NAME", new[] { "FOLDER_ANOTHER", "ITEM" })]
    [Arguments("PLUGINNAME:FOLDER_ANOTHER/ITEM", "PLUGINNAME", new[] { "FOLDER_ANOTHER", "ITEM" })]
    public async Task OmniaIdCreationTest(string fullOmniaId, string @namespace, string[] paths) {
        var assetId = new OmniaId(fullOmniaId);
        await Assert.That(assetId.NameSpace.Value).IsEqualTo(@namespace);
        await Assert.That(assetId.Path.Values).IsEquivalentTo(paths);
    }

    [Test]
    [Arguments("pluginName", "folder/item", "pluginName", new[] { "folder", "item" })]
    [Arguments("plugin_Name", "folder/item", "plugin_Name", new[] { "folder", "item" })]
    [Arguments("plugin_Name", "folder_another/item", "plugin_Name", new[] { "folder_another", "item" })]
    [Arguments("pluginName", "folder_another/item", "pluginName", new[] { "folder_another", "item" })]
    [Arguments("PLUGINNAME", "FOLDER/ITEM", "PLUGINNAME", new[] { "FOLDER", "ITEM" })]
    [Arguments("PLUGIN_NAME", "FOLDER/ITEM", "PLUGIN_NAME", new[] { "FOLDER", "ITEM" })]
    [Arguments("PLUGIN_NAME", "FOLDER_ANOTHER/ITEM", "PLUGIN_NAME", new[] { "FOLDER_ANOTHER", "ITEM" })]
    [Arguments("PLUGINNAME", "FOLDER_ANOTHER/ITEM", "PLUGINNAME", new[] { "FOLDER_ANOTHER", "ITEM" })]
    public async Task OmniaIdThoughFullStringsCreationTest(string pluginId, string assetName, string @namespace, string[] paths) {
        var assetId = new OmniaId(pluginId, assetName);
        
        await Assert.That(assetId.NameSpace.Value).IsEqualTo(@namespace);
        await Assert.That(assetId.Path.Values).IsEquivalentTo(paths);
    }


    [Test]
    [Arguments("pluginName", new[] { "folder", "item" }, "pluginName", new[] { "folder", "item" })]
    [Arguments("plugin_Name", new[] { "folder", "item" }, "plugin_Name", new[] { "folder", "item" })]
    [Arguments("plugin_Name", new[] { "folder_another", "item" }, "plugin_Name", new[] { "folder_another", "item" })]
    [Arguments("pluginName", new[] { "folder_another", "item" }, "pluginName", new[] { "folder_another", "item" })]
    [Arguments("PLUGINNAME", new[] { "FOLDER", "ITEM" }, "PLUGINNAME", new[] { "FOLDER", "ITEM" })]
    [Arguments("PLUGIN_NAME", new[] { "FOLDER", "ITEM" }, "PLUGIN_NAME", new[] { "FOLDER", "ITEM" })]
    [Arguments("PLUGIN_NAME", new[] { "FOLDER_ANOTHER", "ITEM" }, "PLUGIN_NAME", new[] { "FOLDER_ANOTHER", "ITEM" })]
    [Arguments("PLUGINNAME", new[] { "FOLDER_ANOTHER", "ITEM" }, "PLUGINNAME", new[] { "FOLDER_ANOTHER", "ITEM" })]
    [Arguments("PluginName", new[] { "Folder", "Item" }, "PluginName", new[] { "Folder", "Item" })]
    public async Task OmniaIdThoughStringPartsCreationTest(string pluginId, IEnumerable<string> assetName, string @namespace, string[] paths) {
        var assetId = new OmniaId(pluginId, assetName);
        
        await Assert.That(assetId.NameSpace.Value).IsEqualTo(@namespace);
        await Assert.That(assetId.Path.Values).IsEquivalentTo(paths);
    }

    [Test]
    [Arguments("pluginName:folder/")]
    [Arguments("pluginName:folder-")]
    [Arguments("pluginName:folder_")]
    [Arguments("pluginName:")]
    [Arguments("pluginName/folder/")]
    [Arguments("pluginName_:alpha")]
    [Arguments("pluginName-:alpha")]
    [Arguments("PluginName:Folder.Item")]
    [Arguments("PLUGINNAME:FOLDER_ANOTHER.ITEM")]
    public async Task OmniaIdCreateThrowsTest(string input) {
        await Assert.ThrowsAsync<ArgumentException>(() => Task.FromResult(new OmniaId(input)));
    }

    [Test]
    [Arguments("pluginName:folder/")]
    [Arguments("pluginName:folder-")]
    [Arguments("pluginName:folder_")]
    [Arguments("pluginName:")]
    [Arguments("pluginName/folder/")]
    [Arguments("pluginName_:alpha")]
    [Arguments("pluginName-:alpha")]
    [Arguments("PluginName:Folder.Item")]
    [Arguments(@"pluginName:folder\")]
    [Arguments(@"pluginName\folder\")]
    public async Task OmniaIdCreateFailsTest(string input) {
        await Assert.That(OmniaId.TryCreateNew(input, out var output)).IsFalse();
        await Assert.That(output).IsNull();
    }


    [Test]
    [Arguments("pluginName:folder/item", "PLUGINNAME:FOLDER/ITEM")]
    public async Task OmniaIdEqualityTest(string a, string b) {
        var assetA = new OmniaId(a);
        var assetB = new OmniaId(b);

        await Assert.That(assetA).IsEqualTo(assetB);
    }

    [Test]
    [Arguments("pluginName:folder/item", "PLUGINNAME:FOLDER/other")]
    public async Task OmniaId_EqualityFail_Test(string a, string b) {
        var assetA = new OmniaId(a);
        var assetB = new OmniaId(b);
        
        await Assert.That(assetA).IsNotEqualTo(assetB);
    }


    [Test]
    [Arguments("pluginName:folder/item", "pluginName", "folder/item")]
    public async Task OmniaId_Plus_Test(string expectedResult, string pluginId, string assetName) {
        var left = new AssetNameSpace(pluginId);
        var right = new AssetPath(assetName);
        var newAsset = left + right;

        await Assert.That(newAsset.ToString()).IsEqualTo(expectedResult);
    }

    [Test]
    public async Task OmniaId_IsEmpty() {
        var assetId = OmniaId.Empty;
        
        await Assert.That(assetId.IsEmpty).IsTrue();
    }

    [Test]
    public async Task OmniaId_IsEmpty_Not() {
        var assetId = new OmniaId("pluginName:folder/item");
        
        await Assert.That(assetId.IsEmpty).IsFalse();
    }
}
