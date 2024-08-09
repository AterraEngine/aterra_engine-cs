﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;

namespace AterraCore.Contracts.Threading.Logic.EventDelegates;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IChangeActiveLevelEventArgs {
    public AssetId NewLevelId { get; }
}