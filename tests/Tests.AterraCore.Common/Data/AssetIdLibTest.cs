// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.AterraCore.Common.Data;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(AssetIdStringLib))]
public class AssetIdLibTest {
    [Fact]
    public void TestComponentConstants() {
        Assert.Equal("AterraLib:Components/DirectChildren", AssetIdStringLib.AterraLib.Components.DirectChildren);
        Assert.Equal("AterraLib:Components/RaylibCamera2D", AssetIdStringLib.AterraLib.Components.RaylibCamera2D);
        Assert.Equal("AterraLib:Components/Impulse2D", AssetIdStringLib.AterraLib.Components.Impulse2D);
        Assert.Equal("AterraLib:Components/RaylibHudText", AssetIdStringLib.AterraLib.Components.RaylibHudText);
        Assert.Equal("AterraLib:Components/RaylibHudTextPro", AssetIdStringLib.AterraLib.Components.RaylibHudTextPro);
        Assert.Equal("AterraLib:Components/Sprite2D", AssetIdStringLib.AterraLib.Components.Sprite2D);
        Assert.Equal("AterraLib:Components/SystemIds", AssetIdStringLib.AterraLib.Components.SystemIds);
        Assert.Equal("AterraLib:Components/Transform2D", AssetIdStringLib.AterraLib.Components.Transform2D);
    }

    [Fact]
    public void TestEntityConstants() {
        Assert.Equal("AterraLib:Entities/Actor2D", AssetIdStringLib.AterraLib.Entities.Actor2D);
        Assert.Equal("AterraLib:Entities/Camera2D", AssetIdStringLib.AterraLib.Entities.Camera2D);
        Assert.Equal("AterraLib:Entities/EmptyEntity", AssetIdStringLib.AterraLib.Entities.EmptyEntity);
        Assert.Equal("AterraLib:Entities/EmptySprite", AssetIdStringLib.AterraLib.Entities.EmptySprite);
        Assert.Equal("AterraLib:Entities/EmptyLevel", AssetIdStringLib.AterraLib.Entities.EmptyLevel);
        Assert.Equal("AterraLib:Entities/Hud", AssetIdStringLib.AterraLib.Entities.Hud);
        Assert.Equal("AterraLib:Entities/Player2D", AssetIdStringLib.AterraLib.Entities.Player2D);
        Assert.Equal("AterraLib:Entities/LevelRoot", AssetIdStringLib.AterraLib.Entities.LevelRoot);
    }

    [Fact]
    public void TestSystemsLogicConstants() {
        Assert.Equal("AterraLib:SystemsLogic/ApplyImpulse", AssetIdStringLib.AterraLib.SystemsLogic.ApplyImpulse);
        Assert.Equal("AterraLib:SystemsLogic/ApplyImpulseCamera", AssetIdStringLib.AterraLib.SystemsLogic.ApplyImpulseCamera);
        Assert.Equal("AterraLib:SystemsLogic/PlayerController", AssetIdStringLib.AterraLib.SystemsLogic.PlayerController);
        Assert.Equal("AterraLib:SystemsLogic/CameraController", AssetIdStringLib.AterraLib.SystemsLogic.CameraController);
    }

    [Fact]
    public void TestSystemsRenderingConstants() {
        Assert.Equal("AterraLib:SystemsRendering/Render2D", AssetIdStringLib.AterraLib.SystemsRendering.Render2D);
        Assert.Equal("AterraLib:SystemsRendering/RenderHud", AssetIdStringLib.AterraLib.SystemsRendering.RenderHud);
    }
}
