// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.AterraCore.Common.Data;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(Paths))]
public class PathsTest {
    [Fact]
    public void TestConfigEnginePath() {
        Assert.Equal("engine-config.xml", Paths.ConfigEngine);
    }

    [Fact]
    public void TestLogsPaths() {
        Assert.Equal("logs", Paths.Logs.Folder);
        Assert.Equal(Path.Combine("logs", "log_startup-.log"), Paths.Logs.StartupLog);
        Assert.Equal(Path.Combine("logs", "log_engine-.log"), Paths.Logs.EngineLog);
        Assert.Equal(Path.Combine("logs", "log_renderer-.log"), Paths.Logs.RendererLog);
    }

    [Fact]
    public void TestPluginsPaths() {
        Assert.Equal("plugins", Paths.Plugins.Folder);
        Assert.Equal("plugin-config.xml", Paths.Plugins.PluginConfig);
        Assert.Equal("bin", Paths.Plugins.PluginBinFolder);
    }

    [Fact]
    public void TestXsdPaths() {
        Assert.Equal("xsd", Paths.Xsd.Folder);
        Assert.Equal(Path.Combine("xsd", "engine-config.xsd"), Paths.Xsd.XsdEngineConfigDto);
        Assert.Equal(Path.Combine("xsd", "plugin-config.xsd"), Paths.Xsd.XsdPluginConfigDto);
    }
}
