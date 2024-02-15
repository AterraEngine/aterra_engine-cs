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
    [Fact]
    public void TestSemanticVersionConstructorWithIntParams() {
        var semanticVersion = new SemanticVersion(1, 2, 3);
        Assert.Equal(1, semanticVersion.Major);
        Assert.Equal(2, semanticVersion.Minor);
        Assert.Equal(3, semanticVersion.Patch);
    }

    [Fact]
    public void TestSemanticVersionConstructorWithStringParam_ValidString() {
        var semanticVersion = new SemanticVersion("1.2.3");
        Assert.Equal(1, semanticVersion.Major);
        Assert.Equal(2, semanticVersion.Minor);
        Assert.Equal(3, semanticVersion.Patch);
    }

    [Fact]
    public void TestSemanticVersionConstructorWithStringParam_InvalidString() {
        Assert.Throws<ArgumentException>(() => new SemanticVersion("1.2.3.4"));
        Assert.Throws<ArgumentException>(() => new SemanticVersion("1.2"));
        Assert.Throws<ArgumentException>(() => new SemanticVersion("abcd"));
    }

    [Fact]
    public void TestToString() {
        var semanticVersion = new SemanticVersion(1, 2, 3);
        Assert.Equal("1.2.3", semanticVersion.ToString());
    }

    [Fact]
    public void TestReadXml_ValidString() {
        const string xmlContent = "<SemanticVersion>1.2.3</SemanticVersion>";
        var xmlReader = XmlReader.Create(new StringReader(xmlContent));
        
        var semanticVersion = SemanticVersion.Zero;
        while (xmlReader.Read()) {
            if (xmlReader.NodeType != XmlNodeType.Element) continue;

            if (xmlReader.Name == "SemanticVersion") {
                semanticVersion.ReadXml(xmlReader);
            }
        }
    
        Assert.Equal(1, semanticVersion.Major);
        Assert.Equal(2, semanticVersion.Minor);
        Assert.Equal(3, semanticVersion.Patch);
    }

    // [Fact]
    // public void TestWriteXml() {
    //     var semanticVersion = new SemanticVersion(1, 2, 3);
    //     var writer = XmlWriter.Create();
    //     semanticVersion.WriteXml(writer);
    //     Assert.Equal("1.2.3", writer.GetWrittenString());
    // }
}