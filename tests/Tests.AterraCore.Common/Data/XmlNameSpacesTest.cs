// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.AterraCore.Common.Data;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(XmlNameSpaces))]
public class XmlNameSpacesTest {
    [Fact]
    public void TestConfigPlugin() {
        Assert.Equal("urn:aterra-engine:plugin-config", XmlNameSpaces.ConfigPlugin);
    }

    [Fact]
    public void TestConfigEngine() {
        Assert.Equal("urn:aterra-engine:engine-config", XmlNameSpaces.ConfigEngine);
    }
}
