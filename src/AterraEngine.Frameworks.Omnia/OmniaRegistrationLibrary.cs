// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace AterraEngine.Frameworks.Omnia;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
using RegistrationsDictionary = FrozenDictionary<IOmniaRegistrationKey, IOmniaRegistration>;

public class OmniaRegistrationLibrary : IOmniaRegistrationLibrary {
    public required RegistrationsDictionary Registrations { private get; init; }

    // TODO look into performance impact of always having the alternate lookups in memory
    public required RegistrationsDictionary.AlternateLookup<OmniaId> RegistrationsByOmniaId { private get; init; }
    public required RegistrationsDictionary.AlternateLookup<Type> RegistrationsByType { private get; init; }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Lookup Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryGetRegistration(OmniaId omniaId,[NotNullWhen(true)] out IOmniaRegistration? registration) {
        registration = null;
        if (omniaId.IsEmpty) return false;
        
        return RegistrationsByOmniaId.TryGetValue(omniaId, out registration);
    }
    
    public bool TryGetRegistration(Type assetType, [NotNullWhen(true)] out IOmniaRegistration? registration) 
        => RegistrationsByType.TryGetValue(assetType, out registration);
}
