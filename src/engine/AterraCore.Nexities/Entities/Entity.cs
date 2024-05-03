﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Collections.Concurrent;
using AterraCore.Contracts.Nexities.Components;
using AterraCore.Contracts.Nexities.Entities;
using AterraCore.Nexities.Assets;

namespace AterraCore.Nexities.Entities;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class Entity(params IComponent[]? components) : RawEntity<IComponent>(components);