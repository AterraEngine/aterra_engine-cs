// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Xml;
using JetBrains.Annotations;

namespace AterraEngine.Tests.Core.Types;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(SemanticVersion))]
public class SemanticVersionTest {
    
    // Test for constructor with integer parameters and ToString method
    [Theory]
    [InlineData(1, 2, 3, "1.2.3")]
    [InlineData(10, 20, 30, "10.20.30")]
    public void TestSemanticVersionWithIntParams(int major, int minor, int patch, string expected) {
        var semanticVersion = new SemanticVersion(major, minor, patch);
        Assert.Equal(major, semanticVersion.Major);
        Assert.Equal(minor, semanticVersion.Minor);
        Assert.Equal(patch, semanticVersion.Patch);
        Assert.Equal(expected, semanticVersion.ToString());
    }

    // Test for constructor with string parameter
    [Theory]
    [InlineData("1.2.3", 1, 2, 3)]
    [InlineData("10.20.30", 10, 20, 30)]
    public void TestSemanticVersionConstructorWithStringParam_ValidString(string version, int major, int minor, int patch) {
        var semanticVersion = new SemanticVersion(version);
        Assert.Equal(major, semanticVersion.Major);
        Assert.Equal(minor, semanticVersion.Minor);
        Assert.Equal(patch, semanticVersion.Patch);
    }

    // Test for constructor with invalid string parameter
    [Theory]
    [InlineData("1.2.3.4")]
    [InlineData("1.2")]
    [InlineData("abcd")]
    public void TestSemanticVersionConstructorWithStringParam_InvalidString(string version) {
        Assert.Throws<ArgumentException>(() => new SemanticVersion(version));
    }

    // Test for ReadXml method
    [Theory]
    [InlineData("<SemanticVersion>1.2.3</SemanticVersion>", 1, 2, 3)]
    [InlineData("<SemanticVersion>10.20.30</SemanticVersion>", 10, 20, 30)]
    public void TestReadXml_ValidString(string xmlContent, int major, int minor, int patch) {
        var xmlReader = XmlReader.Create(new StringReader(xmlContent));
        SemanticVersion semanticVersion = SemanticVersion.Zero;

        while (xmlReader.Read()) {
            if (xmlReader.NodeType != XmlNodeType.Element) continue;
            if (xmlReader.Name == "SemanticVersion") semanticVersion.ReadXml(xmlReader);
        }

        Assert.Equal(major, semanticVersion.Major);
        Assert.Equal(minor, semanticVersion.Minor);
        Assert.Equal(patch, semanticVersion.Patch);
    }

}