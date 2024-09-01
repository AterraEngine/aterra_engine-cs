// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.AterraCore.Common.Data;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(CoreTagsTest))]
public class CoreTagsTest {
    [Fact]
    public void TestIndividualTags() {
        Assert.Equal((ulong)1 << 0, (ulong)CoreTags.Asset);
        Assert.Equal((ulong)1 << 1, (ulong)CoreTags.Component);
        Assert.Equal((ulong)1 << 2, (ulong)CoreTags.Entity);
        Assert.Equal((ulong)1 << 3, (ulong)CoreTags.System);
        Assert.Equal((ulong)1 << 4, (ulong)CoreTags.RenderSystem);
        Assert.Equal((ulong)1 << 5, (ulong)CoreTags.LogicSystem);
        Assert.Equal((ulong)1 << 6, (ulong)CoreTags.Texture);
        Assert.Equal((ulong)1 << 7, (ulong)CoreTags.Singleton);
        Assert.Equal((ulong)1 << 8, (ulong)CoreTags.Level);
    }

    [Fact]
    public void TestCombinedTags() {
        CoreTags combinedFlag = CoreTags.Asset | CoreTags.Component | CoreTags.Entity;
        Assert.True(combinedFlag.HasFlag(CoreTags.Asset));
        Assert.True(combinedFlag.HasFlag(CoreTags.Component));
        Assert.True(combinedFlag.HasFlag(CoreTags.Entity));
        Assert.False(combinedFlag.HasFlag(CoreTags.System));

        combinedFlag |= CoreTags.System;
        Assert.True(combinedFlag.HasFlag(CoreTags.System));
    }

    [Fact]
    public void TestBitwiseOperations() {
        // Set and test multiple flags
        CoreTags tags = CoreTags.Asset | CoreTags.Texture;
        Assert.Equal((ulong)CoreTags.Asset, (ulong)tags & (ulong)CoreTags.Asset);
        Assert.Equal((ulong)CoreTags.Texture, (ulong)tags & (ulong)CoreTags.Texture);

        // Add another flag and test
        tags |= CoreTags.Entity;
        Assert.Equal((ulong)CoreTags.Entity, (ulong)tags & (ulong)CoreTags.Entity);

        // Remove a flag and test
        tags &= ~CoreTags.Asset;
        Assert.NotEqual((ulong)CoreTags.Asset, (ulong)tags & (ulong)CoreTags.Asset);
    }
}
