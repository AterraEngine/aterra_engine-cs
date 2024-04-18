// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using JetBrains.Annotations;
using Xunit;
using System;

namespace AterraEngine.Tests.Core.Types;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(PluginId))]
public class PluginIdTest {

    [Fact]
    public void PluginIdIntConstructorTest() {
        // Max ushort value test case
        var id = new PluginId(65535);
        Assert.Equal(65535, id.Value);

        // Negative input throwing exception test case
        Assert.Throws<ArgumentOutOfRangeException>(() => new PluginId(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => new PluginId((int)ushort.MaxValue + 1));
    }

    [Fact]
    public void PluginIdStringConstructorTest() {
        // Max ushort value hexadecimal string test case
        var id = new PluginId("FFFF");
        Assert.Equal(65535, id.Value);

        // Invalid hexadecimal string test case
        Assert.Throws<ArgumentException>(() => new PluginId("GHIJ"));
    }

    [Fact]
    public void ToStringTest() {
        // Test conversion to hexadecimal string with padding
        var id = new PluginId(255);
        Assert.Equal("00FF", id.ToString());
    }

    [Fact]
    public void EqualityTest() {
        var id1 = new PluginId(100);
        var id2 = new PluginId(100);
        var id3 = new PluginId(200);

        Assert.True(id1 == id2);
        Assert.False(id1 == id3);
        Assert.True(id1 != id3);
    }

    [Fact]
    public void CompareToTest() {
        var id1 = new PluginId(100);
        var id2 = new PluginId(100);
        var id3 = new PluginId(200);
        var id4 = new PluginId(50);

        Assert.Equal(0, id1.CompareTo(id2));
        Assert.True(id1.CompareTo(id3) < 0);
        Assert.True(id1.CompareTo(id4) > 0);
    }

    [Fact]
    public void EqualsTest() {
        var id1 = new PluginId(100);
        var id2 = new PluginId(100);
        var id3 = new PluginId(200);

        Assert.True(id1.Equals(id2));
        Assert.False(id1.Equals(id3));
    }

}