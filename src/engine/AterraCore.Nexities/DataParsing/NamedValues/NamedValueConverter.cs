// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Nexities.DataParsing.NamedValues;

using Contracts.Nexities.DataParsing.NamedValues;
using Extensions;
using JetBrains.Annotations;
using System.Numerics;
using static Contracts.Nexities.DataParsing.NamedValues.NamedValueConvertors;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class NamedValueConverter : INamedValueConverter {
    private readonly Dictionary<string, Delegate> _map = new() {
        { NamedValueConvertors.ToString, new TryParse<string>(NamedValueConverterMethods.ConvertToString) },
        { ToVector2, new TryParse<Vector2>(NamedValueConverterMethods.ConvertToVector2) },
        { ToVector3, new TryParse<Vector3>(NamedValueConverterMethods.ConvertToVector3) },
        { ToGuid, new TryParse<Guid>(NamedValueConverterMethods.ConvertToGuid) }
    };

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryRegisterProcess(string convertorId, Delegate callback) => _map.TryAdd(convertorId, callback);
    public bool TryRegisterProcess(string convertorId, Delegate callback, bool overwrite) =>
        overwrite
            ? _map.TryAddOrUpdate(convertorId, callback)
            : _map.TryAdd(convertorId, callback);

    public Delegate GetProcessor(string convertor) =>
        _map.TryGetValue(convertor, out Delegate? callback)
            ? callback
            : new TryParse<string>(NamedValueConverterMethods.ConvertToString);
}