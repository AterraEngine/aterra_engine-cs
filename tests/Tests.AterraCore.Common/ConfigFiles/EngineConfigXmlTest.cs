// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles;
using System.Xml.Serialization;

namespace Tests.AterraCore.Common.ConfigFiles;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(EngineConfigXmlTest))]
public class EngineConfigXmlTest {
    [Fact]
    public void EngineConfigXml_ShouldInitializeWithDefaultValues() {
        var configXml = new EngineConfigXml();

        Assert.NotNull(configXml.LoggingConfig);
        Assert.NotNull(configXml.LoadOrder);
        Assert.Equal(Paths.Plugins.Folder, configXml.LoadOrder.RootFolderRelative);
        Assert.NotNull(configXml.LoadOrder.Plugins);
        Assert.Empty(configXml.LoadOrder.Plugins);
    }

    [Fact]
    public void EngineConfigXml_ShouldSerializeToXmlCorrectly() {
        var configXml = new EngineConfigXml {
            LoggingConfig = new EngineConfigXml.LoggingConfigDto {
                UseAsyncConsole = false,
                OutputFilePath = "log.txt"
            },
            LoadOrder = new EngineConfigXml.LoadOrderDto {
                RootFolderRelative = "plugins",
                Plugins = [
                    new EngineConfigXml.LoadOrderDto.PluginDto { FileName = "plugin1.dll" },
                    new EngineConfigXml.LoadOrderDto.PluginDto { FileName = "plugin2.dll" }
                ]
            }
        };

        var serializer = new XmlSerializer(typeof(EngineConfigXml));
        using var stringWriter = new StringWriter();
        serializer.Serialize(stringWriter, configXml);
        string xml = stringWriter.ToString();

        Assert.Contains("<engineConfig", xml);
        Assert.Contains("<logging asyncConsole=\"false\" outputFile=\"log.txt\" />", xml);
        Assert.Contains("<loadOrder relativeRootPath=\"plugins\"", xml);
        Assert.Contains("<plugin file=\"plugin1.dll\" />", xml);
        Assert.Contains("<plugin file=\"plugin2.dll\" />", xml);
    }

    [Fact]
    public void EngineConfigXml_ShouldDeserializeFromXmlCorrectly() {
        const string xml = """
            <engineConfig>
                <logging asyncConsole="true" outputFile="log.txt" />
                <loadOrder relativeRootPath="plugins">
                    <plugin file="plugin1.dll" />
                    <plugin file="plugin2.dll" />
                </loadOrder>
            </engineConfig>
            """;

        var serializer = new XmlSerializer(typeof(EngineConfigXml));
        using var stringReader = new StringReader(xml);
        var configXml = (EngineConfigXml)serializer.Deserialize(stringReader)!;

        Assert.NotNull(configXml);
        Assert.True(configXml.LoggingConfig.UseAsyncConsole);
        Assert.Equal("log.txt", configXml.LoggingConfig.OutputFilePath);
        Assert.Equal("plugins", configXml.LoadOrder.RootFolderRelative);
        Assert.NotNull(configXml.LoadOrder.Plugins);
        Assert.Equal(2, configXml.LoadOrder.Plugins.Length);
        Assert.Equal("plugin1.dll", configXml.LoadOrder.Plugins[0].FileName);
        Assert.Equal("plugin2.dll", configXml.LoadOrder.Plugins[1].FileName);
    }
}
