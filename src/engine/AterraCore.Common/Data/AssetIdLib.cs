﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Common.Data;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class AssetIdLib {
    public static class AterraCore {
        private const string ThisSection = nameof(AterraCore);

        #region Components
        public static class Components {
            private const string ThisClass = nameof(Components);
            
            public const string DirectChildren = $"{ThisSection}:{ThisClass}/{nameof(DirectChildren)}";
            public const string Impulse2D = $"{ThisSection}:{ThisClass}/{nameof(Impulse2D)}";
            public const string RaylibHudText = $"{ThisSection}:{ThisClass}/{nameof(RaylibHudText)}";
            public const string RaylibHudTextPro = $"{ThisSection}:{ThisClass}/{nameof(RaylibHudTextPro)}";
            public const string Sprite2D = $"{ThisSection}:{ThisClass}/{nameof(Sprite2D)}";
            public const string SystemIds = $"{ThisSection}:{ThisClass}/{nameof(SystemIds)}";
            public const string Transfrom2D = $"{ThisSection}:{ThisClass}/{nameof(Transfrom2D)}";
            public const string Transfrom3D = $"{ThisSection}:{ThisClass}/{nameof(Transfrom3D)}";
        }
        #endregion
        #region Entities
        public static class Entities {
            private const string ThisClass = nameof(Entities); 
            
            public const string Actor2D = $"{ThisSection}:{ThisClass}/{nameof(Actor2D)}";
            public const string EmptyEntity = $"{ThisSection}:{ThisClass}/{nameof(EmptyEntity)}";
            public const string EmptyLevel = $"{ThisSection}:{ThisClass}/{nameof(EmptyLevel)}";
            public const string Hud = $"{ThisSection}:{ThisClass}/{nameof(Hud)}";
            public const string Player2D = $"{ThisSection}:{ThisClass}/{nameof(Player2D)}";
        }
        #endregion
        #region Systems - Logic
        public static class SystemsLogic {
            private const string ThisClass = nameof(SystemsLogic);
            
            public const string ApplyImpulse = $"{ThisSection}:{ThisClass}/{nameof(ApplyImpulse)}";
            public const string PlayerController = $"{ThisSection}:{ThisClass}/{nameof(PlayerController)}";
        }
        #endregion
        #region Systems - Rendering
        public static class SystemsRendering {
            private const string ThisClass = nameof(SystemsRendering);
            
            public const string Render2D = $"{ThisSection}:{ThisClass}/{nameof(Render2D)}";
            public const string RenderHud = $"{ThisSection}:{ThisClass}/{nameof(RenderHud)}";
        }
        #endregion
    }
}
