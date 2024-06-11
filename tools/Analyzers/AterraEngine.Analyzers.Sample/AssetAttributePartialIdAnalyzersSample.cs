// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace AterraEngine.Analyzer.Sample;

using AterraCore.Common.Types.Nexities;
using AterraCore.Nexities.Assets;
using AterraCore.Nexities.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Asset("1", ServiceLifetimeType.Singleton, CoreTags.Asset)]
public class TestEntity3 : NexitiesEntity;

[Asset("2", ServiceLifetimeType.Singleton, CoreTags.Asset)]
public class TestEntity4 : NexitiesEntity;

[Asset("3", ServiceLifetimeType.Singleton, CoreTags.Asset)]
public class TestEntity6 : NexitiesEntity;

[Asset("5", ServiceLifetimeType.Singleton, CoreTags.Asset)]
public class TestEntaity4 : NexitiesEntity;