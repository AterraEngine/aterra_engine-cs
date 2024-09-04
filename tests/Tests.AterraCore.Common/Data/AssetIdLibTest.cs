// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.AterraCore.Common.Data;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(AssetIdLib))]
public class AssetIdLibTest {
    [Fact]
    public void TestComponentConstants() {
        Assert.Equal("AterraLib:Components/DirectChildren", AssetIdLib.AterraLib.Components.DirectChildren);
        Assert.Equal("AterraLib:Components/RaylibCamera2D", AssetIdLib.AterraLib.Components.RaylibCamera2D);
        Assert.Equal("AterraLib:Components/Impulse2D", AssetIdLib.AterraLib.Components.Impulse2D);
        Assert.Equal("AterraLib:Components/RaylibHudText", AssetIdLib.AterraLib.Components.RaylibHudText);
        Assert.Equal("AterraLib:Components/RaylibHudTextPro", AssetIdLib.AterraLib.Components.RaylibHudTextPro);
        Assert.Equal("AterraLib:Components/Sprite2D", AssetIdLib.AterraLib.Components.Sprite2D);
        Assert.Equal("AterraLib:Components/SystemIds", AssetIdLib.AterraLib.Components.SystemIds);
        Assert.Equal("AterraLib:Components/Transform2D", AssetIdLib.AterraLib.Components.Transform2D);
    }

    [Fact]
    public void TestEntityConstants() {
        Assert.Equal("AterraLib:Entities/Actor2D", AssetIdLib.AterraLib.Entities.Actor2D);
        Assert.Equal("AterraLib:Entities/Camera2D", AssetIdLib.AterraLib.Entities.Camera2D);
        Assert.Equal("AterraLib:Entities/EmptyEntity", AssetIdLib.AterraLib.Entities.EmptyEntity);
        Assert.Equal("AterraLib:Entities/EmptySprite", AssetIdLib.AterraLib.Entities.EmptySprite);
        Assert.Equal("AterraLib:Entities/EmptyLevel", AssetIdLib.AterraLib.Entities.EmptyLevel);
        Assert.Equal("AterraLib:Entities/Hud", AssetIdLib.AterraLib.Entities.Hud);
        Assert.Equal("AterraLib:Entities/Player2D", AssetIdLib.AterraLib.Entities.Player2D);
        Assert.Equal("AterraLib:Entities/LevelRoot", AssetIdLib.AterraLib.Entities.LevelRoot);
    }

    [Fact]
    public void TestSystemsLogicConstants() {
        Assert.Equal("AterraLib:SystemsLogic/ApplyImpulse", AssetIdLib.AterraLib.SystemsLogic.ApplyImpulse);
        Assert.Equal("AterraLib:SystemsLogic/ApplyImpulseCamera", AssetIdLib.AterraLib.SystemsLogic.ApplyImpulseCamera);
        Assert.Equal("AterraLib:SystemsLogic/PlayerController", AssetIdLib.AterraLib.SystemsLogic.PlayerController);
        Assert.Equal("AterraLib:SystemsLogic/CameraController", AssetIdLib.AterraLib.SystemsLogic.CameraController);
    }

    [Fact]
    public void TestSystemsRenderingConstants() {
        Assert.Equal("AterraLib:SystemsRendering/Render2D", AssetIdLib.AterraLib.SystemsRendering.Render2D);
        Assert.Equal("AterraLib:SystemsRendering/RenderHud", AssetIdLib.AterraLib.SystemsRendering.RenderHud);
    }
}
