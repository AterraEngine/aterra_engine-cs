// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

using AterraCore.Common.Nexities;
using AterraCore.Nexities.Entities;

namespace AterraEngine.Analyzer.Sample;

// [Entity("1", AssetInstanceType.Pooled)]
// public class TestEntity : Entity;

[Entity<TestEntity2>("1", AssetInstanceType.Singleton)]
public class TestEntity2 : NexitiesEntity;