// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

using AterraCore.Common.Nexities;
using AterraCore.Nexities.Assets;
using AterraCore.Nexities.Entities;

namespace AterraEngine.Analyzer.Sample;

// [Entity("1", AssetInstanceType.Pooled))]
// public class TestEntity : NexitiesEntity;

[Entity<TestEntity2>("4", AssetInstanceType.Singleton)]
public class TestEntity2 : NexitiesEntity;