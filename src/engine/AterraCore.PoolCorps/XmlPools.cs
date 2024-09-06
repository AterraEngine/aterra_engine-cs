// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Attributes;
using AterraCore.Contracts.PoolCorps;
using JetBrains.Annotations;
using Microsoft.Extensions.ObjectPool;
using System.Xml;

namespace AterraCore.PoolCorps;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[Singleton<IXmlPools>]
public class XmlPools : IXmlPools {
    private const int InitialCapacity = 24;
    private readonly DefaultObjectPoolProvider _objectPoolProvider = new();
    
    #region XmlNodeQueuePool
    private ObjectPool<Queue<XmlNode>>? _xmlNodeQueuePool;
    public ObjectPool<Queue<XmlNode>> XmlNodeQueuePool =>
        _xmlNodeQueuePool ??= _objectPoolProvider.Create(new ComponentsByIdPoolPolicy(InitialCapacity));

    private class ComponentsByIdPoolPolicy(int capacity) : PooledObjectPolicy<Queue<XmlNode>> {
        public override Queue<XmlNode> Create() => new(capacity);
        public override bool Return(Queue<XmlNode> obj) {
            obj.Clear();
            return true;
        }
    }
    #endregion
}
