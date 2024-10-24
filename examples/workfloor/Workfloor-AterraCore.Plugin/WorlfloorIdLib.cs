﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Workfloor_AterraCore.Plugin;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class WorkfloorIdLib {
    private const string ThisSection = "Workfloor";

    public static class Tags {
        private const string ThisClass = nameof(Tags);
        public const string EnemyCollection = $"#{ThisSection}:{ThisClass}/{nameof(EnemyCollection)}";
    }

    #region Components
    public static class Components {
        private const string ThisClass = nameof(Components);

        public const string TextureDuckyHype = $"{ThisSection}:{ThisClass}/{nameof(TextureDuckyHype)}";
        public const string SpriteDuckyHype = $"{ThisSection}:{ThisClass}/{nameof(SpriteDuckyHype)}";
        public const string TextureDuckyPlatinum = $"{ThisSection}:{ThisClass}/{nameof(TextureDuckyPlatinum)}";
        public const string SpriteDuckyPlatinum = $"{ThisSection}:{ThisClass}/{nameof(SpriteDuckyPlatinum)}";
        public const string TagComponent = $"{ThisSection}:{ThisClass}/{nameof(TagComponent)}";
        public const string BoundingCircle = $"{ThisSection}:{ThisClass}/{nameof(BoundingCircle)}";
    }
    #endregion
    #region Entities
    public static class Entities {
        private const string ThisClass = nameof(Entities);

        public const string ActorDuckyHype = $"{ThisSection}:{ThisClass}/{nameof(ActorDuckyHype)}";
        public const string ActorDuckyPlatinum = $"{ThisSection}:{ThisClass}/{nameof(ActorDuckyPlatinum)}";
        public const string PropDuckyHype = $"{ThisSection}:{ThisClass}/{nameof(PropDuckyHype)}";
        public const string PropDuckyPlatinum = $"{ThisSection}:{ThisClass}/{nameof(PropDuckyPlatinum)}";
        public const string DuckyPlayer = $"{ThisSection}:{ThisClass}/{nameof(DuckyPlayer)}";
        public const string EntityCollection = $"{ThisSection}:{ThisClass}/{nameof(EntityCollection)}";
    }
    #endregion
    #region Levels
    public static class Levels {
        private const string ThisClass = nameof(Levels);
        public const string Blob = $"{ThisSection}:{ThisClass}/{nameof(Blob)}";
        public const string Main = $"{ThisSection}:{ThisClass}/{nameof(Main)}";
        public const string Game = $"{ThisSection}:{ThisClass}/{nameof(Game)}";
        public const string FractalBarnsley = $"{ThisSection}:{ThisClass}/{nameof(FractalBarnsley)}";
        public const string FractalChaos = $"{ThisSection}:{ThisClass}/{nameof(FractalChaos)}";
        public const string FractalDragon = $"{ThisSection}:{ThisClass}/{nameof(FractalDragon)}";
        public const string FractalSierpinski = $"{ThisSection}:{ThisClass}/{nameof(FractalSierpinski)}";
    }
    #endregion

    #region Systems - Logic
    public static class SystemsLogic {
        private const string ThisClass = nameof(SystemsLogic);

        public const string RandomImpulse = $"{ThisSection}:{ThisClass}/{nameof(RandomImpulse)}";
        public const string LevelSwitch = $"{ThisSection}:{ThisClass}/{nameof(LevelSwitch)}";
        public const string SpawnEnemy = $"{ThisSection}:{ThisClass}/{nameof(SpawnEnemy)}";
        public const string Collision = $"{ThisSection}:{ThisClass}/{nameof(Collision)}";
    }
    #endregion
    #region Systems - Rendering
    public static class SystemsRendering {
        private const string ThisClass = nameof(SystemsRendering);
    }
    #endregion
}
