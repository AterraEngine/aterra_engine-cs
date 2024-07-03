// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace AterraEngine.Analyzer.Sample;

using AterraCore.Common.Types.Nexities;
using AterraCore.Nexities.Entities;

// [Entity("1", AssetInstanceType.Pooled))]
// public class TestEntity : NexitiesEntity;

[Entity<TestEntity2>("4")]
public class TestEntity2 : NexitiesEntity;