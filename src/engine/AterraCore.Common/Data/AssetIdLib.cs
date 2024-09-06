// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;

namespace AterraCore.Common.Data;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class AssetIdLib {
    public static class AterraLib {
        public static readonly AssetId ConfigMancer = AssetIdStringLib.AterraLib.ConfigMancer;

        #region Components
        public static class Components {
            public static readonly AssetId DirectChildren =  AssetIdStringLib.AterraLib.Components.DirectChildren;
            public static readonly AssetId RaylibCamera2D =  AssetIdStringLib.AterraLib.Components.RaylibCamera2D;
            public static readonly AssetId Impulse2D =  AssetIdStringLib.AterraLib.Components.Impulse2D;
            public static readonly AssetId RaylibHudText =  AssetIdStringLib.AterraLib.Components.RaylibHudText;
            public static readonly AssetId RaylibHudTextPro =  AssetIdStringLib.AterraLib.Components.RaylibHudTextPro;
            public static readonly AssetId Sprite2D =  AssetIdStringLib.AterraLib.Components.Sprite2D;
            public static readonly AssetId SystemIds =  AssetIdStringLib.AterraLib.Components.SystemIds;
            public static readonly AssetId Transform2D =  AssetIdStringLib.AterraLib.Components.Transform2D;
        }
        #endregion
        #region Entities
        public static class Entities {
            public static readonly AssetId Actor2D = AssetIdStringLib.AterraLib.Entities.Actor2D;
            public static readonly AssetId Camera2D = AssetIdStringLib.AterraLib.Entities.Camera2D;
            public static readonly AssetId EmptyEntity = AssetIdStringLib.AterraLib.Entities.EmptyEntity;
            public static readonly AssetId EmptySprite = AssetIdStringLib.AterraLib.Entities.EmptySprite;
            public static readonly AssetId EmptyLevel = AssetIdStringLib.AterraLib.Entities.EmptyLevel;
            public static readonly AssetId Hud = AssetIdStringLib.AterraLib.Entities.Hud;
            public static readonly AssetId Player2D = AssetIdStringLib.AterraLib.Entities.Player2D;
            public static readonly AssetId LevelRoot = AssetIdStringLib.AterraLib.Entities.LevelRoot;
        }
        #endregion
        #region Systems - Logic
        public static class SystemsLogic {
            public static readonly AssetId ApplyImpulse = AssetIdStringLib.AterraLib.SystemsLogic.ApplyImpulse;
            public static readonly AssetId ApplyImpulseCamera = AssetIdStringLib.AterraLib.SystemsLogic.ApplyImpulseCamera;
            public static readonly AssetId PlayerController = AssetIdStringLib.AterraLib.SystemsLogic.PlayerController;
            public static readonly AssetId CameraController = AssetIdStringLib.AterraLib.SystemsLogic.CameraController;
        }
        #endregion
        #region Systems - Rendering
        public static class SystemsRendering {
            public static readonly AssetId Render2D = AssetIdStringLib.AterraLib.SystemsRendering.Render2D;
            public static readonly AssetId RenderHud = AssetIdStringLib.AterraLib.SystemsRendering.RenderHud;
            public static readonly AssetId RaylibKeyHandler = AssetIdStringLib.AterraLib.SystemsRendering.RaylibKeyHandler;
        }
        #endregion
    }
}
