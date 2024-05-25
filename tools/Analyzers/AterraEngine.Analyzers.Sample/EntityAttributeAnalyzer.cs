// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

using AterraCore.Common.Nexities;

namespace AterraEngine.Analyzer.Sample;

using AterraCore.Nexities.Data.Entities;

// [Entity("1", AssetInstanceType.Pooled))]
// public class TestEntity : NexitiesEntity;

[Entity<TestEntity2>("4", AssetInstanceType.Singleton)]
public class TestEntity2 : NexitiesEntity;