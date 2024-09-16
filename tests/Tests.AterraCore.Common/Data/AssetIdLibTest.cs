// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.AterraCore.Common.Data;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(StringAssetIdLib))]
public class AssetIdLibTest {
    [Fact]
    public void TestComponentConstants() {
        Assert.Equal("AterraLib:Components/DirectChildren", StringAssetIdLib.AterraLib.Components.DirectChildren);
        Assert.Equal("AterraLib:Components/RaylibCamera2D", StringAssetIdLib.AterraLib.Components.RaylibCamera2D);
        Assert.Equal("AterraLib:Components/Impulse2D", StringAssetIdLib.AterraLib.Components.Impulse2D);
        Assert.Equal("AterraLib:Components/RaylibHudText", StringAssetIdLib.AterraLib.Components.RaylibHudText);
        Assert.Equal("AterraLib:Components/RaylibHudTextPro", StringAssetIdLib.AterraLib.Components.RaylibHudTextPro);
        Assert.Equal("AterraLib:Components/Sprite2D", StringAssetIdLib.AterraLib.Components.Sprite2D);
        Assert.Equal("AterraLib:Components/SystemIds", StringAssetIdLib.AterraLib.Components.SystemIds);
        Assert.Equal("AterraLib:Components/Transform2D", StringAssetIdLib.AterraLib.Components.Transform2D);
    }

    [Fact]
    public void TestEntityConstants() {
        Assert.Equal("AterraLib:Entities/Actor2D", StringAssetIdLib.AterraLib.Entities.Actor2D);
        Assert.Equal("AterraLib:Entities/Camera2D", StringAssetIdLib.AterraLib.Entities.Camera2D);
        Assert.Equal("AterraLib:Entities/EmptyEntity", StringAssetIdLib.AterraLib.Entities.EmptyEntity);
        Assert.Equal("AterraLib:Entities/EmptySprite", StringAssetIdLib.AterraLib.Entities.EmptySprite);
        Assert.Equal("AterraLib:Entities/EmptyLevel", StringAssetIdLib.AterraLib.Entities.EmptyLevel);
        Assert.Equal("AterraLib:Entities/Hud", StringAssetIdLib.AterraLib.Entities.Hud);
        Assert.Equal("AterraLib:Entities/Player2D", StringAssetIdLib.AterraLib.Entities.Player2D);
        Assert.Equal("AterraLib:Entities/LevelRoot", StringAssetIdLib.AterraLib.Entities.LevelRoot);
    }

    [Fact]
    public void TestSystemsLogicConstants() {
        Assert.Equal("AterraLib:SystemsLogic/ApplyImpulse", StringAssetIdLib.AterraLib.SystemsLogic.ApplyImpulse);
        Assert.Equal("AterraLib:SystemsLogic/ApplyImpulseCamera", StringAssetIdLib.AterraLib.SystemsLogic.ApplyImpulseCamera);
        Assert.Equal("AterraLib:SystemsLogic/PlayerController", StringAssetIdLib.AterraLib.SystemsLogic.PlayerController);
        Assert.Equal("AterraLib:SystemsLogic/CameraController", StringAssetIdLib.AterraLib.SystemsLogic.CameraController);
    }

    [Fact]
    public void TestSystemsRenderingConstants() {
        Assert.Equal("AterraLib:SystemsRendering/Render2D", StringAssetIdLib.AterraLib.SystemsRendering.Render2D);
        Assert.Equal("AterraLib:SystemsRendering/RenderUi", StringAssetIdLib.AterraLib.SystemsRendering.RenderUi);
    }
}
