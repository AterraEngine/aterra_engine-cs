// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using JetBrains.Annotations;

namespace AterraEngine.Tests.Core.Types;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(PluginId))]
public class PluginIdTest {

    [Theory]
    [InlineData(-1)]
    [InlineData(65536)]
    public void ConstructorTest_InvalidInputs(int value) {
        Assert.Throws<ArgumentOutOfRangeException>(() => new PluginId(value));
    }

    [Theory]
    [InlineData("10000")]
    [InlineData("abcg")]
    [InlineData("fffff")]
    [InlineData("-123")]
    public void ConstructorTest_InvalidStringInputs(string value) {
        Assert.Throws<ArgumentException>(() => new PluginId(value));
    }

    [Theory]
    [InlineData(0, "0000")]
    [InlineData(15, "000F")]
    [InlineData(256, "0100")]
    [InlineData(65535, "FFFF")]
    public void ToStringTest(ushort value, string expected) {
        PluginId id = new(value);
        Assert.Equal(expected, id.ToString());
    }

    [Theory]
    [InlineData(100, 100, true)]
    [InlineData(100, 200, false)]
    public void EqualityTest(ushort value1, ushort value2, bool expected) {
        PluginId id1 = new(value1);
        PluginId id2 = new(value2);

        // Test == and != operators, Equals() and GetHashCode()
        Assert.Equal(expected, id1 == id2);
        Assert.Equal(expected, id1.Equals(id2));
        Assert.Equal(expected, id1.Equals((object)id2));
        Assert.Equal(expected, id1.GetHashCode() == id2.GetHashCode());
        Assert.Equal(!expected, id1 != id2);
    }

    [Theory]
    [InlineData(100, 100, 0)]
    [InlineData(100, 200, -100)]
    [InlineData(200, 100, 100)]
    public void ComparerTest(ushort value1, ushort value2, int expected) {
        PluginId id1 = new(value1);
        PluginId id2 = new(value2);

        Assert.Equal(expected, id1.CompareTo(id2));
    }

    [Theory]
    [InlineData(100, 100, true)]
    [InlineData(100, 200, false)]
    public void EqualityComparerTest(ushort value1, ushort value2, bool expected) {
        PluginId id1 = new(value1);
        PluginId id2 = new(value2);

        Assert.Equal(expected, id1.Equals(id1, id2));
        Assert.Equal(expected, id1.GetHashCode(id1) == id2.GetHashCode(id2));
    }
}