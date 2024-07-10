// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.FlexiPlug;
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Boot.Logic.PluginLoading;
using System.Reflection;

namespace AterraCore.Boot.Logic.PluginLoading;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// --------------------------------------------------------------------------------------------------------------------
public abstract class APluginDto : IPluginDto {
    private PluginValidity _validity = PluginValidity.Valid;
    public PluginValidity Validity {
        get => _validity;
        set => _validity = _validity != PluginValidity.Invalid ? value : _validity; // Once invalid, always invalid
    }

    public abstract IEnumerable<Type> Types { get; }
    public abstract PluginId PluginId { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void SetInvalid() {
        _validity = PluginValidity.Invalid;
    }
    
    public IEnumerable<(Type Type, T Attribute)> GetOfAttribute<T>() where T : Attribute {
        return Types
            .SelectMany(type => type
                .GetCustomAttributes<T>(false)// this way we only get the attribute once
                .Select(attribute => (Type: type, Attribute: attribute))
            );
    }
}
