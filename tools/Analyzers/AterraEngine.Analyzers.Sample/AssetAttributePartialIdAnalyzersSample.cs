// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

using AterraCore.Common.Nexities;

namespace AterraEngine.Analyzer.Sample;

using AterraCore.Nexities.Data.Assets;
using AterraCore.Nexities.Data.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Asset("1", AssetInstanceType.Singleton, CoreTags.Asset)]
public class TestEntity3 : NexitiesEntity;

[Asset("2", AssetInstanceType.Singleton, CoreTags.Asset)]
public class TestEntity4 : NexitiesEntity;

[Asset("3", AssetInstanceType.Singleton, CoreTags.Asset)]
public class TestEntity6 : NexitiesEntity;

[Asset("5", AssetInstanceType.Singleton, CoreTags.Asset)]
public class TestEntaity4 : NexitiesEntity;