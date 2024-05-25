// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

using AterraCore.Common.Nexities;

namespace AterraEngine.Analyzer.Sample;

using AterraCore.Nexities.Entities;

// [Entity("1", AssetInstanceType.Pooled))]
// public class TestEntity : NexitiesEntity;

[Entity<TestEntity2>("4", ServiceLifetimeType.Singleton)]
public class TestEntity2 : NexitiesEntity;