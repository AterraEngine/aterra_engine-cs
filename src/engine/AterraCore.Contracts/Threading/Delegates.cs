// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.World;
using System.Diagnostics.CodeAnalysis;

namespace AterraCore.Contracts.Threading;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public delegate void TickEventHandler(IActiveLevel activeLevel);
public delegate void EmptyEventHandler();

public delegate void LevelChangeStarted(IActiveLevel oldLevel);
public delegate void LevelChangeCompleted(IActiveLevel newLevel);

public delegate bool TryGetSystemsDelegate<T>([NotNullWhen(true)] out T[]? systems);