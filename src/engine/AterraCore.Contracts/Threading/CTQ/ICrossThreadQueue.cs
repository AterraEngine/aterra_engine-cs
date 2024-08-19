﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Threading.CTQ.Dto;
using System.Collections.Concurrent;

namespace AterraCore.Contracts.Threading.CTQ;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public interface ICrossThreadQueue {
    ConcurrentQueue<TextureRegistrar> TextureRegistrarQueue { get; }
}