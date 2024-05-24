// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Nexities.SaveFileSystems.NamedValues;

using Contracts.SaveFileSystem.NamedValues;
using Extensions;
using JetBrains.Annotations;
using System.Numerics;
using static Contracts.SaveFileSystem.NamedValues.NamedValueConvertors;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class NamedValueConverter : INamedValueConverter{
    private readonly Dictionary<ulong, Delegate> _map = new() {
        {(ulong) NamedValueConvertors.ToString, new TryParse<string>(NamedValueConverterMethods.ConvertToString)},
        {(ulong) ToVector2, new TryParse<Vector2>(NamedValueConverterMethods.ConvertToVector2)},
        {(ulong) ToVector3, new TryParse<Vector3>(NamedValueConverterMethods.ConvertToVector3)},
        {(ulong) ToGuid, new TryParse<Guid>(NamedValueConverterMethods.ConvertToGuid)}
    };

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryRegisterProcess(ulong convertorId, Delegate callback) {
        return _map.TryAdd(convertorId, callback);
    }
    public bool TryRegisterProcess(ulong convertorId, Delegate callback, bool overwrite) {
        return overwrite 
            ? _map.TryAddOrUpdate(convertorId, callback) 
            : _map.TryAdd(convertorId, callback);
    }

    public Delegate GetProcessor(NamedValueConvertors convertor) => GetProcessor((ulong)convertor);    
    public Delegate GetProcessor(ulong convertor) =>
        _map.TryGetValue(convertor, out Delegate? callback) 
            ? callback
            : new TryParse<string>(NamedValueConverterMethods.ConvertToString);
}
