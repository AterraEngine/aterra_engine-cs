﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Common.Data;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class StringAssetIdLib {
    public static class AterraCore {
        private const string ThisSection = nameof(AterraCore);

        #region CrossThreadDataHolders
        public static class CrossThreadDataHolders {
            private const string ThisClass = nameof(CrossThreadDataHolders);

            public const string DataCollector = $"{ThisSection}:{ThisClass}/{nameof(DataCollector)}";
            public const string TextureBus = $"{ThisSection}:{ThisClass}/{nameof(TextureBus)}";
            public const string LevelChangeBus = $"{ThisSection}:{ThisClass}/{nameof(LevelChangeBus)}";
        }
        #endregion
    }

    public static class AterraLib {
        private const string ThisSection = nameof(AterraLib);
        public const string ConfigMancer = $"{ThisSection}:{nameof(ConfigMancer)}";

        #region CrossThreadDataHolders
        public static class CrossThreadDataHolders {
            private const string ThisClass = nameof(CrossThreadDataHolders);

            public const string TickDataInput = $"{ThisSection}:{ThisClass}/{nameof(TickDataInput)}";
            public const string RenderableData = $"{ThisSection}:{ThisClass}/{nameof(RenderableData)}";
        }
        #endregion

        #region Components
        public static class Components {
            private const string ThisClass = nameof(Components);

            public const string DirectChildren = $"{ThisSection}:{ThisClass}/{nameof(DirectChildren)}";
            public const string RaylibCamera2D = $"{ThisSection}:{ThisClass}/{nameof(RaylibCamera2D)}";
            public const string Impulse2D = $"{ThisSection}:{ThisClass}/{nameof(Impulse2D)}";
            public const string RaylibHudText = $"{ThisSection}:{ThisClass}/{nameof(RaylibHudText)}";
            public const string RaylibHudTextPro = $"{ThisSection}:{ThisClass}/{nameof(RaylibHudTextPro)}";
            public const string Sprite2D = $"{ThisSection}:{ThisClass}/{nameof(Sprite2D)}";
            public const string SystemIds = $"{ThisSection}:{ThisClass}/{nameof(SystemIds)}";
            public const string Transform2D = $"{ThisSection}:{ThisClass}/{nameof(Transform2D)}";
        }
        #endregion
        #region Entities
        public static class Entities {
            private const string ThisClass = nameof(Entities);

            public const string Actor2D = $"{ThisSection}:{ThisClass}/{nameof(Actor2D)}";
            public const string Camera2D = $"{ThisSection}:{ThisClass}/{nameof(Camera2D)}";
            public const string EmptyEntity = $"{ThisSection}:{ThisClass}/{nameof(EmptyEntity)}";
            public const string EmptySprite = $"{ThisSection}:{ThisClass}/{nameof(EmptySprite)}";
            public const string EmptyLevel = $"{ThisSection}:{ThisClass}/{nameof(EmptyLevel)}";
            public const string Hud = $"{ThisSection}:{ThisClass}/{nameof(Hud)}";
            public const string Player2D = $"{ThisSection}:{ThisClass}/{nameof(Player2D)}";
            public const string LevelRoot = $"{ThisSection}:{ThisClass}/{nameof(LevelRoot)}";
            public const string Prop2D = $"{ThisSection}:{ThisClass}/{nameof(Prop2D)}";
        }
        #endregion
        #region Systems - Logic
        public static class SystemsLogic {
            private const string ThisClass = nameof(SystemsLogic);

            public const string ApplyImpulse = $"{ThisSection}:{ThisClass}/{nameof(ApplyImpulse)}";
            public const string ApplyImpulseCamera = $"{ThisSection}:{ThisClass}/{nameof(ApplyImpulseCamera)}";
            public const string PlayerController = $"{ThisSection}:{ThisClass}/{nameof(PlayerController)}";
            public const string CameraController = $"{ThisSection}:{ThisClass}/{nameof(CameraController)}";
        }
        #endregion
        #region Systems - Rendering
        public static class SystemsRendering {
            private const string ThisClass = nameof(SystemsRendering);

            public const string Render2D = $"{ThisSection}:{ThisClass}/{nameof(Render2D)}";
            public const string Render2DPrepForProps = $"{ThisSection}:{ThisClass}/{nameof(Render2DPrepForProps)}";
            public const string Render2DPrepForActors = $"{ThisSection}:{ThisClass}/{nameof(Render2DPrepForActors)}";
            public const string RenderUi = $"{ThisSection}:{ThisClass}/{nameof(RenderUi)}";
            public const string RaylibKeyHandler = $"{ThisSection}:{ThisClass}/{nameof(RaylibKeyHandler)}";
        }
        #endregion
    }
}
