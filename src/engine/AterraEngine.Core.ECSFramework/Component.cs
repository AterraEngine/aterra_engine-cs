﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Core.Assets;
using AterraEngine.Contracts.Core.ECSFramework;
using AterraEngine.Core.Assets;
using Serilog;

namespace AterraEngine.Core.ECSFramework;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class Component<TDto>(ILogger logger) : Asset<TDto>(logger), IComponent where TDto : class {
    
}