// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.Types.Nexities;
using AterraCore.Contracts.Boot.Logic.PluginLoading;
using System.Reflection;

namespace AterraCore.Boot.Logic.PluginLoading;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// --------------------------------------------------------------------------------------------------------------------
public abstract class APluginDto : IPluginDto {

    // Once invalid, always invalid
    public abstract IEnumerable<Type> Types { get; }
    public abstract PluginId PluginId { get; }
    public bool IsValid { get; private set; } = true;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void SetInvalid() {
        IsValid = false;
    }
    
    public IEnumerable<(Type Type, T Attribute)> GetOfAttribute<T>() where T : Attribute {
        return Types
            .SelectMany(type => type
                .GetCustomAttributes<T>(false)// this way we only get the attribute once
                .Select(attribute => (Type: type, Attribute: attribute))
            );
    }
}
