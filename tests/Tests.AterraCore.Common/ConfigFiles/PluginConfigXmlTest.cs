// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles;
using System.Xml.Serialization;

namespace Tests.AterraCore.Common.ConfigFiles;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(PluginConfigXmlTest))]
public class PluginConfigXmlTest {
    [Fact]
    public void PluginConfigXml_ShouldInitializeWithDefaultValues() {
        var configXml = new PluginConfigXml();

        Assert.Equal(string.Empty, configXml.Author);
        Assert.Equal(string.Empty, configXml.PluginVersionValue);
        Assert.Equal(string.Empty, configXml.GameVersionValue);
        Assert.NotNull(configXml.BinDtos);
        Assert.Empty(configXml.BinDtos);
    }

    [Fact]
    public void PluginConfigXml_ShouldSerializeToXmlCorrectly() {
        var configXml = new PluginConfigXml {
            NameSpace = "TestNamespace",
            NameReadable = "Test Plugin",
            Author = "Test Author",
            PluginVersionValue = "1.0.0",
            GameVersionValue = "1.2.3",
            BinDtos = [
                new PluginConfigXml.BinDto { FileName = "file1.dll" },
                new PluginConfigXml.BinDto { FileName = "file2.dll" }
            ]
        };

        var serializer = new XmlSerializer(typeof(PluginConfigXml));
        using var stringWriter = new StringWriter();

        serializer.Serialize(stringWriter, configXml);
        string xml = stringWriter.ToString();

        Assert.Contains("<pluginConfig", xml);
        Assert.Contains("<nameSpace>TestNamespace</nameSpace>", xml);
        Assert.Contains("<nameReadable>Test Plugin</nameReadable>", xml);
        Assert.Contains("<author>Test Author</author>", xml);
        Assert.Contains("<pluginVersion>1.0.0</pluginVersion>", xml);
        Assert.Contains("<expectedGameVersion>1.2.3</expectedGameVersion>", xml);
        Assert.Contains("<bins>", xml);
        Assert.Contains("<bin file=\"file1.dll\" />", xml);
        Assert.Contains("<bin file=\"file2.dll\" />", xml);
    }

    [Fact]
    public void PluginConfigXml_ShouldDeserializeFromXmlCorrectly() {
        const string xml = """
            <pluginConfig>
                <nameSpace>TestNamespace</nameSpace>
                <nameReadable>Test Plugin</nameReadable>
                <author>Test Author</author>
                <pluginVersion>1.0.0</pluginVersion>
                <expectedGameVersion>1.2.3</expectedGameVersion>
                <bins>
                    <bin file="file1.dll" />
                    <bin file="file2.dll" />
                </bins>
            </pluginConfig>
            """;

        var serializer = new XmlSerializer(typeof(PluginConfigXml));
        using var stringReader = new StringReader(xml);

        var configXml = (PluginConfigXml)serializer.Deserialize(stringReader)!;

        Assert.NotNull(configXml);
        Assert.Equal("TestNamespace", configXml.NameSpace);
        Assert.Equal("Test Plugin", configXml.NameReadable);
        Assert.Equal("Test Author", configXml.Author);
        Assert.Equal("1.0.0", configXml.PluginVersionValue);
        Assert.Equal("1.2.3", configXml.GameVersionValue);
        Assert.NotNull(configXml.BinDtos);
        Assert.Equal(2, configXml.BinDtos.Length);
        Assert.Equal("file1.dll", configXml.BinDtos[0].FileName);
        Assert.Equal("file2.dll", configXml.BinDtos[1].FileName);
    }

    [Fact]
    public void PluginConfigXml_ShouldCacheSemanticVersions() {
        var configXml = new PluginConfigXml {
            PluginVersionValue = "1.0.0",
            GameVersionValue = "1.2.3"
        };

        SemanticVersion pluginVersion1 = configXml.PluginVersion;
        SemanticVersion pluginVersion2 = configXml.PluginVersion;
        SemanticVersion gameVersion1 = configXml.GameVersion;
        SemanticVersion gameVersion2 = configXml.GameVersion;

        Assert.Equal(pluginVersion1, pluginVersion2);
        Assert.Equal(gameVersion1, gameVersion2);
        Assert.Equal("1.0.0", pluginVersion1.ToString());
        Assert.Equal("1.2.3", gameVersion1.ToString());
    }
}
