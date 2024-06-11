// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Nexities.DataParsing.NamedValues;
using CodeOfChaos.Extensions;
using JetBrains.Annotations;
using System.Numerics;

namespace AterraCore.Nexities.Parsers.NamedValues;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class NamedValueConverter : INamedValueConverter {
    private readonly Dictionary<string, Delegate> _map = new() {
        { NamedValueConvertors.ToString, new TryParse<string>(NamedValueConverterMethods.ConvertToString) },
        { NamedValueConvertors.ToVector2, new TryParse<Vector2>(NamedValueConverterMethods.ConvertToVector2) },
        { NamedValueConvertors.ToVector3, new TryParse<Vector3>(NamedValueConverterMethods.ConvertToVector3) },
        { NamedValueConvertors.ToGuid, new TryParse<Guid>(NamedValueConverterMethods.ConvertToGuid) }
    };

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryRegisterProcess(string convertorId, Delegate callback) => _map.TryAdd(convertorId, callback);
    public bool TryRegisterProcess(string convertorId, Delegate callback, bool overwrite) {
        if (!overwrite) return _map.TryAdd(convertorId, callback);
        _map.AddOrUpdate(convertorId, callback);
        return true;
    }

    public Delegate GetProcessor(string convertor) =>
        _map.TryGetValue(convertor, out Delegate? callback)
            ? callback
            : new TryParse<string>(NamedValueConverterMethods.ConvertToString);
}
