// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Common.Data;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class AssetIdLib {
    public static class AterraCore {
        public static class CrossThreadDataHolders {
            public static readonly AssetId DataCollector = StringAssetIdLib.AterraCore.CrossThreadDataHolders.DataCollector;
            public static readonly AssetId TextureBus = StringAssetIdLib.AterraCore.CrossThreadDataHolders.TextureBus;
            public static readonly AssetId LevelChangeBus = StringAssetIdLib.AterraCore.CrossThreadDataHolders.LevelChangeBus;
        }
    }
    
    public static class AterraLib {
        public static readonly AssetId ConfigMancer = StringAssetIdLib.AterraLib.ConfigMancer;

        #region Components
        public static class Components {
            public static readonly AssetId DirectChildren = StringAssetIdLib.AterraLib.Components.DirectChildren;
            public static readonly AssetId RaylibCamera2D = StringAssetIdLib.AterraLib.Components.RaylibCamera2D;
            public static readonly AssetId Impulse2D = StringAssetIdLib.AterraLib.Components.Impulse2D;
            public static readonly AssetId RaylibHudText = StringAssetIdLib.AterraLib.Components.RaylibHudText;
            public static readonly AssetId RaylibHudTextPro = StringAssetIdLib.AterraLib.Components.RaylibHudTextPro;
            public static readonly AssetId Sprite2D = StringAssetIdLib.AterraLib.Components.Sprite2D;
            public static readonly AssetId SystemIds = StringAssetIdLib.AterraLib.Components.SystemIds;
            public static readonly AssetId Transform2D = StringAssetIdLib.AterraLib.Components.Transform2D;
        }
        #endregion
        #region Entities
        public static class Entities {
            public static readonly AssetId Actor2D = StringAssetIdLib.AterraLib.Entities.Actor2D;
            public static readonly AssetId Camera2D = StringAssetIdLib.AterraLib.Entities.Camera2D;
            public static readonly AssetId EmptyEntity = StringAssetIdLib.AterraLib.Entities.EmptyEntity;
            public static readonly AssetId EmptySprite = StringAssetIdLib.AterraLib.Entities.EmptySprite;
            public static readonly AssetId EmptyLevel = StringAssetIdLib.AterraLib.Entities.EmptyLevel;
            public static readonly AssetId Hud = StringAssetIdLib.AterraLib.Entities.Hud;
            public static readonly AssetId Player2D = StringAssetIdLib.AterraLib.Entities.Player2D;
            public static readonly AssetId LevelRoot = StringAssetIdLib.AterraLib.Entities.LevelRoot;
            public static readonly AssetId Prop2D = StringAssetIdLib.AterraLib.Entities.Prop2D;
        }
        #endregion
        #region Systems - Logic
        public static class SystemsLogic {
            public static readonly AssetId ApplyImpulse = StringAssetIdLib.AterraLib.SystemsLogic.ApplyImpulse;
            public static readonly AssetId ApplyImpulseCamera = StringAssetIdLib.AterraLib.SystemsLogic.ApplyImpulseCamera;
            public static readonly AssetId PlayerController = StringAssetIdLib.AterraLib.SystemsLogic.PlayerController;
            public static readonly AssetId CameraController = StringAssetIdLib.AterraLib.SystemsLogic.CameraController;
            public static readonly AssetId PostLogicProcessing = StringAssetIdLib.AterraLib.SystemsLogic.PostLogicProcessing;
        }
        #endregion
        #region Systems - Rendering
        public static class SystemsRendering {
            public static readonly AssetId Render2D = StringAssetIdLib.AterraLib.SystemsRendering.Render2D;
            public static readonly AssetId PostRendering = StringAssetIdLib.AterraLib.SystemsRendering.PostRendering;
            public static readonly AssetId Render2DPrepForProps = StringAssetIdLib.AterraLib.SystemsRendering.Render2DPrepForProps;
            public static readonly AssetId Render2DPrepForActors = StringAssetIdLib.AterraLib.SystemsRendering.Render2DPrepForActors;
            public static readonly AssetId RenderUi = StringAssetIdLib.AterraLib.SystemsRendering.RenderUi;
            public static readonly AssetId RaylibKeyHandler = StringAssetIdLib.AterraLib.SystemsRendering.RaylibKeyHandler;
        }
        #endregion
    }
}
