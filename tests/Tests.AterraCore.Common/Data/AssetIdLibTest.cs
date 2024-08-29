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
        Assert.Equal("AterraCore:Components/DirectChildren", AssetIdLib.AterraCore.Components.DirectChildren);
        Assert.Equal("AterraCore:Components/RaylibCamera2D", AssetIdLib.AterraCore.Components.RaylibCamera2D);
        Assert.Equal("AterraCore:Components/Impulse2D", AssetIdLib.AterraCore.Components.Impulse2D);
        Assert.Equal("AterraCore:Components/RaylibHudText", AssetIdLib.AterraCore.Components.RaylibHudText);
        Assert.Equal("AterraCore:Components/RaylibHudTextPro", AssetIdLib.AterraCore.Components.RaylibHudTextPro);
        Assert.Equal("AterraCore:Components/Sprite2D", AssetIdLib.AterraCore.Components.Sprite2D);
        Assert.Equal("AterraCore:Components/SystemIds", AssetIdLib.AterraCore.Components.SystemIds);
        Assert.Equal("AterraCore:Components/Transform2D", AssetIdLib.AterraCore.Components.Transform2D);
    }

    [Fact]
    public void TestEntityConstants() {
        Assert.Equal("AterraCore:Entities/Actor2D", AssetIdLib.AterraCore.Entities.Actor2D);
        Assert.Equal("AterraCore:Entities/Camera2D", AssetIdLib.AterraCore.Entities.Camera2D);
        Assert.Equal("AterraCore:Entities/EmptyEntity", AssetIdLib.AterraCore.Entities.EmptyEntity);
        Assert.Equal("AterraCore:Entities/EmptySprite", AssetIdLib.AterraCore.Entities.EmptySprite);
        Assert.Equal("AterraCore:Entities/EmptyLevel", AssetIdLib.AterraCore.Entities.EmptyLevel);
        Assert.Equal("AterraCore:Entities/Hud", AssetIdLib.AterraCore.Entities.Hud);
        Assert.Equal("AterraCore:Entities/Player2D", AssetIdLib.AterraCore.Entities.Player2D);
        Assert.Equal("AterraCore:Entities/LevelRoot", AssetIdLib.AterraCore.Entities.LevelRoot);
    }

    [Fact]
    public void TestSystemsLogicConstants() {
        Assert.Equal("AterraCore:SystemsLogic/ApplyImpulse", AssetIdLib.AterraCore.SystemsLogic.ApplyImpulse);
        Assert.Equal("AterraCore:SystemsLogic/ApplyImpulseCamera", AssetIdLib.AterraCore.SystemsLogic.ApplyImpulseCamera);
        Assert.Equal("AterraCore:SystemsLogic/PlayerController", AssetIdLib.AterraCore.SystemsLogic.PlayerController);
        Assert.Equal("AterraCore:SystemsLogic/CameraController", AssetIdLib.AterraCore.SystemsLogic.CameraController);
    }

    [Fact]
    public void TestSystemsRenderingConstants() {
        Assert.Equal("AterraCore:SystemsRendering/Render2D", AssetIdLib.AterraCore.SystemsRendering.Render2D);
        Assert.Equal("AterraCore:SystemsRendering/RenderHud", AssetIdLib.AterraCore.SystemsRendering.RenderHud);
    }
}
