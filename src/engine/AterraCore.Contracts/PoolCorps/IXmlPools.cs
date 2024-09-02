﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.ObjectPool;
using System.Xml;

namespace AterraCore.Contracts.PoolCorps;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IXmlPools {
    ObjectPool<Queue<XmlNode>> XmlNodeQueuePool { get; }
}
